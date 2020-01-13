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
using System.Reflection;

using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Identifies a single plugin to be activated in the system.
    /// </summary>
    /// <seealso cref="BlueBoxMoon.Data.EntityFramework.IEntityPlugin" />
    public class EntityPlugin
    {
        #region Properties

        /// <summary>
        /// Gets the unique identifier for this plugin.
        /// </summary>
        /// <value>
        /// The unique identifier for this plugin.
        /// </value>
        public string Identifier { get; }

        /// <summary>
        /// Gets the migrations that are associated with this plugin.
        /// </summary>
        /// <value>
        /// The migrations that are associated with this plugin.
        /// </value>
        public IReadOnlyDictionary<string, TypeInfo> Migrations { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPlugin"/> class.
        /// </summary>
        /// <param name="identifier">The unique identifier of the plugin.</param>
        /// <param name="migrationTypes">The migration class types.</param>
        public EntityPlugin( string identifier, IEnumerable<Type> migrationTypes )
        {
            Identifier = identifier;

            var migrations = new Dictionary<string, TypeInfo>();

            foreach ( var type in migrationTypes )
            {
                var migrationId = type.GetCustomAttribute<MigrationAttribute>()?.Id;

                if ( !string.IsNullOrWhiteSpace( migrationId ) )
                {
                    migrations.Add( migrationId, type.GetTypeInfo() );
                }
            }

            Migrations = migrations;
        }

        #endregion
    }
}
