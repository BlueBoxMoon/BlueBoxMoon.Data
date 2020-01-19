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

using BlueBoxMoon.Data.EntityFramework.Migrations;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Provides additional functionality to the <see cref="DatabaseFacade"/> class.
    /// </summary>
    public static class EntityDatabaseFacadeExtensions
    {
        /// <summary>
        /// Migrates a plugin to the latest version possible.
        /// </summary>
        /// <param name="databaseFacade">The database facade.</param>
        /// <param name="plugin">The plugin to be migrated.</param>
        public static void MigratePlugin( this DatabaseFacade databaseFacade, EntityPlugin plugin )
        {
            var migrator = ( ( IInfrastructure<IServiceProvider> ) databaseFacade ).Instance.GetService<IPluginMigrator>();

            migrator.Migrate( plugin );
        }

        /// <summary>
        /// Migrates all plugins to the latest version possible.
        /// </summary>
        /// <param name="databaseFacade">The database facade.</param>
        public static void MigratePlugins( this DatabaseFacade databaseFacade )
        {
            var migrator = ( ( IInfrastructure<IServiceProvider> ) databaseFacade ).Instance.GetService<IPluginMigrator>();
            var currentContext = ( ( IInfrastructure<IServiceProvider> ) databaseFacade ).Instance.GetService<ICurrentDbContext>();
            var context = ( EntityDbContext ) currentContext.Context;

            foreach ( var plugin in context.EntityContextOptions.Plugins )
            {
                migrator.Migrate( plugin );
            }
        }

        /// <summary>
        /// Initializes a plugin and gives it a chance to perform any database operations.
        /// </summary>
        /// <param name="databaseFacade">The database facade.</param>
        /// <param name="plugin">The plugin to be initialized.</param>
        public static void InitializePlugin( this DatabaseFacade databaseFacade, EntityPlugin plugin )
        {
            var currentContext = ( ( IInfrastructure<IServiceProvider> ) databaseFacade ).Instance.GetService<ICurrentDbContext>();
            var context = ( EntityDbContext ) currentContext.Context;

            plugin.Initialize( context );
        }

        /// <summary>
        /// Initializes all plugins and gives them a chance to perform any database operations.
        /// </summary>
        /// <param name="databaseFacade">The database facade.</param>
        public static void InitializePlugins( this DatabaseFacade databaseFacade )
        {
            var currentContext = ( ( IInfrastructure<IServiceProvider> ) databaseFacade ).Instance.GetService<ICurrentDbContext>();
            var context = ( EntityDbContext ) currentContext.Context;

            foreach ( var plugin in context.EntityContextOptions.Plugins )
            {
                plugin.Initialize( context );
            }
        }
    }
}
