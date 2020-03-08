// MIT License
//
// Copyright( c) 2020 Blue Box Moon
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace BlueBoxMoon.Data.EntityFramework.EntityTypes
{
    public class EntityTypesPlugin : EntityPlugin
    {
        /// <summary>
        /// The schema used by this plugin.
        /// </summary>
        internal const string Schema = "EntityTypes";

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        /// <value>
        /// The name of the plugin.
        /// </value>
        public override string Name => "Entity Types";

        /// <summary>
        /// Get the migration types that need to be run to install or uninstall
        /// this plugin.
        /// </summary>
        /// <returns>An enumerable of <see cref="Type"/>.</returns>
        public override IEnumerable<Type> GetMigrations()
        {
            var x = base.GetMigrations().ToList();
            var ns = typeof( Migrations.Initialize ).Namespace;
            return base.GetMigrations()
                .Where( a => a.Namespace == typeof( Migrations.Initialize ).Namespace );
        }

        /// <summary>
        /// Called by application code early in the app life cycle to initialize
        /// all plugins. Will be called sometime after migrations have run.
        /// </summary>
        /// <param name="context">The database context the plugin can make changes in.</param>
        public override void Initialize( EntityDbContext context )
        {
            var entityTypes = context.EntityContextOptions.RegisteredEntities.Keys;
            var dataSet = context.GetDataSet<EntityType>();

            foreach ( var entityType in entityTypes )
            {
                EntityType entity = null;
                var guidAttribute = entityType.GetCustomAttribute<GuidAttribute>();
                Guid? guid = null;

                if ( guidAttribute != null && Guid.TryParse( guidAttribute.Value, out var guidValue ) )
                {
                    guid = guidValue;
                    entity = dataSet.GetByGuid( guid.Value );
                }
                else
                {
                    entity = dataSet.AsQueryable()
                        .Where( a => a.QualifiedName == entityType.AssemblyQualifiedName )
                        .FirstOrDefault();
                }

                if ( entity == null )
                {
                    entity = new EntityType
                    {
                        Guid = guid ?? Guid.NewGuid()
                    };

                    dataSet.Add( entity );
                }

                entity.Name = entityType.FullName;
                entity.QualifiedName = entityType.AssemblyQualifiedName;
            }

            context.SaveChanges();
        }
    }
}
