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

using BlueBoxMoon.Data.EntityFramework.Internals;
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
        /// Gets a list of all <see cref="EntityPlugin"/> objects
        /// registered with the system.
        /// </summary>
        /// <param name="databaseFacade">The database facade.</param>
        /// <returns>An enumeration of all <see cref="EntityPlugin"/> objects.</returns>
        public static IEnumerable<EntityPlugin> GetPlugins( this DatabaseFacade databaseFacade )
        {
            var currentContext = ( ( IInfrastructure<IServiceProvider> ) databaseFacade ).Instance.GetService<ICurrentDbContext>();
            var context = ( EntityDbContext ) currentContext.Context;

            return context.EntityContextOptions.Plugins;
        }

        /// <summary>
        /// Installs a plugin to the latest version possible.
        /// </summary>
        /// <param name="databaseFacade">The database facade.</param>
        /// <param name="pluginType">The plugin to be migrated.</param>
        public static void InstallPlugin( this DatabaseFacade databaseFacade, Type pluginType )
        {
            var currentContext = ( ( IInfrastructure<IServiceProvider> ) databaseFacade ).Instance.GetService<ICurrentDbContext>();
            var context = ( EntityDbContext ) currentContext.Context;
            var plugin = context.EntityContextOptions.Plugins.Single( a => a.GetType() == pluginType );
            var migrator = ( ( IInfrastructure<IServiceProvider> ) databaseFacade ).Instance.GetService<IPluginMigrator>();

            migrator.Migrate( plugin );
        }

        /// <summary>
        /// Installs a plugin to the latest version possible.
        /// </summary>
        /// <typeparam name="TPlugin">The type of the plugin to be installed.</typeparam>
        /// <param name="databaseFacade">The database facade.</param>
        public static void InstallPlugin<TPlugin>( this DatabaseFacade databaseFacade )
            where TPlugin : EntityPlugin
        {
            InstallPlugin( databaseFacade, typeof( TPlugin ) );
        }

        /// <summary>
        /// Remove a single plugin from the database by running all of
        /// its down migration steps until it is completely uninstalled.
        /// </summary>
        /// <param name="databaseFacade">The database facade.</param>
        /// <param name="pluginType">The plugin to be completely removed.</param>
        public static void RemovePlugin( this DatabaseFacade databaseFacade, Type pluginType )
        {
            var currentContext = ( ( IInfrastructure<IServiceProvider> ) databaseFacade ).Instance.GetService<ICurrentDbContext>();
            var context = ( EntityDbContext ) currentContext.Context;
            var plugin = context.EntityContextOptions.Plugins.Single( a => a.GetType() == pluginType );
            var migrator = ( ( IInfrastructure<IServiceProvider> ) databaseFacade ).Instance.GetService<IPluginMigrator>();

            migrator.Migrate( plugin, SemanticVersion.Empty );
        }

        /// <summary>
        /// Remove a single plugin from the database by running all of
        /// its down migration steps until it is completely uninstalled.
        /// </summary>
        /// <param name="databaseFacade">The database facade.</param>
        public static void RemovePlugin<TPlugin>( this DatabaseFacade databaseFacade )
            where TPlugin : EntityPlugin
        {
            RemovePlugin( databaseFacade, typeof( TPlugin ) );
        }

        /// <summary>
        /// Installs all plugins to the latest version possible.
        /// </summary>
        /// <param name="databaseFacade">The database facade.</param>
        public static void InstallPlugins( this DatabaseFacade databaseFacade )
        {
            var migrator = ( ( IInfrastructure<IServiceProvider> ) databaseFacade ).Instance.GetService<IPluginMigrator>();
            var currentContext = ( ( IInfrastructure<IServiceProvider> ) databaseFacade ).Instance.GetService<ICurrentDbContext>();
            var context = ( EntityDbContext ) currentContext.Context;

            migrator.Migrate( context.EntityContextOptions.Plugins );
        }

        /// <summary>
        /// Initializes a plugin and gives it a chance to perform any database operations.
        /// </summary>
        /// <param name="databaseFacade">The database facade.</param>
        /// <param name="pluginType">The plugin to be initialized.</param>
        public static void InitializePlugin( this DatabaseFacade databaseFacade, Type pluginType )
        {
            var currentContext = ( ( IInfrastructure<IServiceProvider> ) databaseFacade ).Instance.GetService<ICurrentDbContext>();
            var context = ( EntityDbContext ) currentContext.Context;
            var plugin = context.EntityContextOptions.Plugins.Single( a => a.GetType() == pluginType );

            plugin.Initialize( context );
        }

        /// <summary>
        /// Initializes a plugin and gives it a chance to perform any database operations.
        /// </summary>
        /// <typeparam name="TPlugin">The type of the plugin to be initialized.</typeparam>
        /// <param name="databaseFacade">The database facade.</param>
        public static void InitializePlugin<TPlugin>( this DatabaseFacade databaseFacade )
            where TPlugin : EntityPlugin
        {
            InitializePlugin( databaseFacade, typeof( TPlugin ) );
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
