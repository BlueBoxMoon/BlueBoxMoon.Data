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

using BlueBoxMoon.Data.EntityFramework.Migrations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Identifies a single plugin to be activated in the system.
    /// </summary>
    /// <seealso cref="BlueBoxMoon.Data.EntityFramework.IEntityPlugin" />
    public abstract class EntityPlugin
    {
        #region Properties

        /// <summary>
        /// Gets the unique identifier for this plugin.
        /// </summary>
        /// <value>
        /// The unique identifier for this plugin.
        /// </value>
        public virtual string Identifier => GetType().Assembly.GetName().Name;

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        /// <value>
        /// The name of the plugin.
        /// </value>
        public virtual string Name => GetType().Assembly.GetName().Name;

        /// <summary>
        /// Get the migration types that need to be run to install or uninstall
        /// this plugin.
        /// </summary>
        /// <returns>An enumerable of <see cref="Type"/>.</returns>
        public virtual IEnumerable<Type> GetMigrations()
        {
            return GetType()
                .Assembly
                .GetExportedTypes()
                .Where( a => typeof( EntityMigration ).IsAssignableFrom( a ) )
                .Where( a => a.GetCustomAttribute<MigrationAttribute>() !=  null )
                .ToList();
        }

        /// <summary>
        /// Called when the EntityDbContext needs to create it's model.
        /// </summary>
        /// <param name="modelBuilder">The instance that handles building the model.</param>
        public virtual void OnModelCreating( ModelBuilder modelBuilder )
        {
        }

        #endregion
    }
}
