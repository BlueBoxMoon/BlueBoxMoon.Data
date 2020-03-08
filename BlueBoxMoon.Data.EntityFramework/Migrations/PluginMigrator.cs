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

using BlueBoxMoon.Data.EntityFramework.Internals;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace BlueBoxMoon.Data.EntityFramework.Migrations
{
    /// <summary>
    /// Handles the tasks necessary to migrate plugin versions.
    /// </summary>
    /// <seealso cref="BlueBoxMoon.Data.EntityFramework.IPluginMigrator" />
    public class PluginMigrator : IPluginMigrator
    {
        #region Properties

        /// <summary>
        /// Gets the migrations SQL generator.
        /// </summary>
        /// <value>
        /// The migrations SQL generator.
        /// </value>
        protected IMigrationsSqlGenerator MigrationsSqlGenerator { get; }

        /// <summary>
        /// Gets the migrations assembly.
        /// </summary>
        /// <value>
        /// The migrations assembly.
        /// </value>
        protected IMigrationsAssembly MigrationsAssembly { get; }

        /// <summary>
        /// Gets the database creator.
        /// </summary>
        /// <value>
        /// The database creator.
        /// </value>
        protected IRelationalDatabaseCreator DatabaseCreator { get; }

        /// <summary>
        /// Gets the plugin history repository.
        /// </summary>
        /// <value>
        /// The plugin history repository.
        /// </value>
        protected IPluginHistoryRepository PluginHistoryRepository { get; }

        /// <summary>
        /// Gets the raw SQL command builder.
        /// </summary>
        /// <value>
        /// The raw SQL command builder.
        /// </value>
        protected IRawSqlCommandBuilder RawSqlCommandBuilder { get; }

        /// <summary>
        /// Gets the migration command executor.
        /// </summary>
        /// <value>
        /// The migration command executor.
        /// </value>
        protected IMigrationCommandExecutor MigrationCommandExecutor { get; }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        protected IRelationalConnection Connection { get; }

        /// <summary>
        /// Gets the SQL generation helper.
        /// </summary>
        /// <value>
        /// The SQL generation helper.
        /// </value>
        protected ISqlGenerationHelper SqlGenerationHelper { get; }

        /// <summary>
        /// Gets the current context.
        /// </summary>
        /// <value>
        /// The current context.
        /// </value>
        protected ICurrentDbContext CurrentContext { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets the command logger.
        /// </summary>
        /// <value>
        /// The command logger.
        /// </value>
        protected IDiagnosticsLogger<DbLoggerCategory.Database.Command> CommandLogger { get; }

        /// <summary>
        /// Gets the database provider.
        /// </summary>
        /// <value>
        /// The database provider.
        /// </value>
        protected IDatabaseProvider DatabaseProvider { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginMigrator"/> class.
        /// </summary>
        /// <param name="migrationsSqlGenerator">The migrations SQL generator.</param>
        /// <param name="migrationsAssembly">The migrations assembly.</param>
        /// <param name="databaseCreator">The database creator.</param>
        /// <param name="pluginHistoryRepository">The plugin history repository.</param>
        /// <param name="rawSqlCommandBuilder">The raw SQL command builder.</param>
        /// <param name="migrationCommandExecutor">The migration command executor.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="sqlGenerationHelper">The SQL generation helper.</param>
        /// <param name="currentContext">The current context.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="commandLogger">The command logger.</param>
        /// <param name="databaseProvider">The database provider.</param>
        public PluginMigrator(
            IMigrationsSqlGenerator migrationsSqlGenerator,
            IMigrationsAssembly migrationsAssembly,
            IDatabaseCreator databaseCreator,
            IPluginHistoryRepository pluginHistoryRepository,
            IRawSqlCommandBuilder rawSqlCommandBuilder,
            IMigrationCommandExecutor migrationCommandExecutor,
            IRelationalConnection connection,
            ISqlGenerationHelper sqlGenerationHelper,
            ICurrentDbContext currentContext,
            ILoggerFactory loggerFactory,
            IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger,
            IDatabaseProvider databaseProvider )
        {
            MigrationsSqlGenerator = migrationsSqlGenerator;
            MigrationsAssembly = migrationsAssembly;
            DatabaseCreator = ( IRelationalDatabaseCreator ) databaseCreator;
            PluginHistoryRepository = pluginHistoryRepository;
            RawSqlCommandBuilder = rawSqlCommandBuilder;
            MigrationCommandExecutor = migrationCommandExecutor;
            Connection = connection;
            SqlGenerationHelper = sqlGenerationHelper;
            CurrentContext = currentContext;
            Logger = loggerFactory.CreateLogger( "BlueBoxMoon.Data.EntityFramework.Migrations.PluginMigrator" );
            CommandLogger = commandLogger;
            DatabaseProvider = databaseProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Ensures the database and plugin history repository exist.
        /// </summary>
        protected virtual void EnsureExists()
        {
            //
            // Verify the history table exists and if not create it.
            //
            if ( !PluginHistoryRepository.Exists() )
            {
                if ( !DatabaseCreator.Exists() )
                {
                    DatabaseCreator.Create();
                }

                var command = RawSqlCommandBuilder.Build( PluginHistoryRepository.GetCreateScript() );

                var query = new RelationalCommandParameterObject( Connection, null, null, CurrentContext.Context, CommandLogger );

                command.ExecuteNonQuery( query );
            }
        }

        /// <summary>
        /// Initiates a migration operation to the target migration version.
        /// </summary>
        /// <param name="plugin">The plugin whose migrations should be run.</param>
        /// <param name="targetMigration">The target migration.</param>
        public virtual void Migrate( EntityPlugin plugin, string targetMigration = null )
        {
            Logger.LogInformation( LoggingEvents.MigratingId, LoggingEvents.Migrating, plugin.Identifier, Connection.DbConnection.Database, Connection.DbConnection.DataSource );

            EnsureExists();

            //
            // Get all the command lists to be executed.
            //
            var commandLists = GetMigrationCommandLists(
                plugin,
                PluginHistoryRepository.GetAppliedMigrations( plugin ),
                targetMigration );

            //
            // Execute each command list in order.
            //
            foreach ( var commandList in commandLists )
            {
                MigrationCommandExecutor.ExecuteNonQuery( commandList(), Connection );
            }
        }

        /// <summary>
        /// Initiates a migration operation of the plugins to the latest versions.
        /// </summary>
        /// <param name="plugin">The plugins whose migrations should be run.</param>
        public void Migrate( IEnumerable<EntityPlugin> plugins )
        {
            foreach ( var plugin in plugins )
            {
                Logger.LogInformation( LoggingEvents.MigratingId, LoggingEvents.Migrating, plugin.Identifier, Connection.DbConnection.Database, Connection.DbConnection.DataSource );
            }

            EnsureExists();

            var migrationNodes = new List<MigrationNode>();
            var appliedMigrations = new List<string>();

            foreach ( var plugin in plugins )
            {
                var appliedMigrationEntries = PluginHistoryRepository.GetAppliedMigrations( plugin );

                appliedMigrations.AddRange( appliedMigrationEntries
                    .Select( a => $"{plugin.Identifier}-{a.MigrationId}" ) );

                //
                // Identify all the migration classes and the migrations to be applied.
                //
                PopulateMigrations(
                    plugin,
                    appliedMigrationEntries.Select( t => t.MigrationId ),
                    null,
                    out var migrationsToApply,
                    out var migrationsToRevert,
                    out var actualTargetMigration );

                var nodes = migrationsToApply.Select( migration =>
                    {
                        var migrationId = migration.GetType().GetCustomAttribute<MigrationAttribute>().Id;
                        Logger.LogInformation( LoggingEvents.ApplyingMigrationId, LoggingEvents.ApplyingMigration, migrationId, plugin.Identifier );

                        return new MigrationNode( plugin, migration, GenerateUpSql( plugin, migration ) );
                    } )
                    .OrderBy( a => a.MigrationId )
                    .ToList();

                //
                // Set a dependency on each migration, skipping the first one.
                //
                for ( int i = 1; i < nodes.Count; i++ )
                {
                    nodes[i].Dependencies.Add( nodes[i - 1] );
                }

                migrationNodes.AddRange( nodes );
            }

            //
            // Add all inter-plugin dependencies.
            //
            foreach ( var node in migrationNodes )
            {
                //
                // Find dependencies for other plugins.
                //
                var dependsOn = node.Migration
                    .GetType()
                    .GetCustomAttributes<DependsOnPluginAttribute>()
                    .Select( b => new
                    {
                        ( ( EntityPlugin ) Activator.CreateInstance( b.PluginType ) ).Identifier,
                        b.MigrationId
                    } )
                    .Where( b => !appliedMigrations.Contains( $"{b.Identifier}-{b.MigrationId}" ) )
                    .Select( b => migrationNodes.FirstOrDefault( c => c.Plugin.Identifier == b.Identifier && c.MigrationId == b.MigrationId ) )
                    .ToList();

                if ( dependsOn.Any( a => a == null ) )
                {
                    throw new DependencyException( node.Plugin.Name, node.MigrationId, $"A required dependency was not found." );
                }

                node.Dependencies.AddRange( dependsOn );
            }

            //
            // Sort migrations taking into account dependencies.
            //
            migrationNodes = migrationNodes.TopologicalSort( a => a.Dependencies ).ToList();

            //
            // Execute each command list in order.
            //
            foreach ( var commandList in migrationNodes.Select( a => a.Commands ) )
            {
                MigrationCommandExecutor.ExecuteNonQuery( commandList, Connection );
            }
        }

        /// <summary>
        /// Gets the migration command lists.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="appliedMigrationEntries">The applied migration entries.</param>
        /// <param name="targetMigration">The target migration.</param>
        /// <returns></returns>
        protected virtual IEnumerable<Func<IReadOnlyList<MigrationCommand>>> GetMigrationCommandLists( EntityPlugin plugin, IReadOnlyList<HistoryRow> appliedMigrationEntries, string targetMigration = null )
        {
            //
            // Identify all the migration classes and whether they need to be
            // applied or reverted.
            //
            PopulateMigrations(
                plugin,
                appliedMigrationEntries.Select( t => t.MigrationId ),
                targetMigration,
                out var migrationsToApply,
                out var migrationsToRevert,
                out var actualTargetMigration );

            //
            // Loop through the migrations to be reverted and generate a
            // lazy loaded enumerable containing them. This needs to be a
            // for-next loop and not a foreach so we can track the previous
            // migration reference.
            //
            for ( int i = 0; i < migrationsToRevert.Count; i++ )
            {
                var migration = migrationsToRevert[i];

                var index = i;
                yield return () =>
                {
                    var migrationId = migration.GetType().GetCustomAttribute<MigrationAttribute>().Id;
                    Logger.LogInformation( LoggingEvents.RevertingMigrationId, LoggingEvents.RevertingMigration, migrationId, plugin.Identifier );

                    var previousMigration = index != migrationsToRevert.Count - 1 ? migrationsToRevert[index + 1] : actualTargetMigration;

                    return GenerateDownSql( plugin, migration, previousMigration );
                };
            }

            //
            // Loop through the migrations to be applied and generate a
            // lazy loaded enumable containing them.
            //
            foreach ( var migration in migrationsToApply )
            {
                yield return () =>
                {
                    var migrationId = migration.GetType().GetCustomAttribute<MigrationAttribute>().Id;
                    Logger.LogInformation( LoggingEvents.ApplyingMigrationId, LoggingEvents.ApplyingMigration, migrationId, plugin.Identifier );

                    return GenerateUpSql( plugin, migration );
                };
            }

            if ( migrationsToRevert.Count + migrationsToApply.Count == 0 )
            {
                Logger.LogInformation( LoggingEvents.MigrationNotNeededId, LoggingEvents.MigrationNotNeeded );
            }
        }

        /// <summary>
        /// Populates the migration lists.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="appliedMigrationEntries">The applied migration entries.</param>
        /// <param name="targetMigration">The target migration.</param>
        /// <param name="migrationsToApply">The migrations to apply.</param>
        /// <param name="migrationsToRevert">The migrations to revert.</param>
        /// <param name="actualTargetMigration">The actual target migration.</param>
        protected virtual void PopulateMigrations(
            EntityPlugin plugin,
            IEnumerable<string> appliedMigrationEntries,
            string targetMigration,
            out IReadOnlyList<Migration> migrationsToApply,
            out IReadOnlyList<Migration> migrationsToRevert,
            out Migration actualTargetMigration )
        {
            var appliedMigrations = new Dictionary<string, TypeInfo>();
            var unappliedMigrations = new Dictionary<string, TypeInfo>();
            var migrations = plugin.GetMigrations().ToList();

            if ( migrations.Count == 0 )
            {
                Logger.LogInformation( LoggingEvents.MigrationsNotFoundId, LoggingEvents.MigrationsNotFound, plugin.Name );
            }

            //
            // Determine the set of applied and unapplied migrations.
            //
            foreach ( var migration in migrations )
            {
                var migrationId = migration.GetCustomAttribute<MigrationAttribute>().Id;

                if ( appliedMigrationEntries.Contains( migrationId ) )
                {
                    appliedMigrations.Add( migrationId, migration.GetTypeInfo() );
                }
                else
                {
                    unappliedMigrations.Add( migrationId, migration.GetTypeInfo() );
                }
            }

            //
            // Build the list of migrations to apply or revert.
            //
            if ( string.IsNullOrEmpty( targetMigration ) )
            {
                //
                // Migrate to latest version.
                //
                migrationsToApply = unappliedMigrations
                    .OrderBy( m => m.Key )
                    .Select( p => MigrationsAssembly.CreateMigration( p.Value, DatabaseProvider.Name ) )
                    .ToList();
                migrationsToRevert = Array.Empty<Migration>();
                actualTargetMigration = null;
            }
            else if ( targetMigration == Migration.InitialDatabase )
            {
                //
                // Migrate to uninstalled.
                //
                migrationsToApply = Array.Empty<Migration>();
                migrationsToRevert = appliedMigrations
                    .OrderByDescending( m => m.Key )
                    .Select( p => MigrationsAssembly.CreateMigration( p.Value, DatabaseProvider.Name ) )
                    .ToList();
                actualTargetMigration = null;
            }
            else
            {
                //
                // Migrate to specific version.
                //
                migrationsToApply = unappliedMigrations
                    .Where( m => string.Compare( m.Key, targetMigration, StringComparison.OrdinalIgnoreCase ) <= 0 )
                    .OrderBy( m => m.Key )
                    .Select( p => MigrationsAssembly.CreateMigration( p.Value, DatabaseProvider.Name ) )
                    .ToList();

                migrationsToRevert = appliedMigrations
                    .Where( m => string.Compare( m.Key, targetMigration, StringComparison.OrdinalIgnoreCase ) > 0 )
                    .OrderByDescending( m => m.Key )
                    .Select( p => MigrationsAssembly.CreateMigration( p.Value, DatabaseProvider.Name ) )
                    .ToList();

                actualTargetMigration = appliedMigrations
                    .Where( m => string.Compare( m.Key, targetMigration, StringComparison.OrdinalIgnoreCase ) == 0 )
                    .Select( p => MigrationsAssembly.CreateMigration( p.Value, DatabaseProvider.Name ) )
                    .SingleOrDefault();
            }
        }

        /// <summary>
        /// Generates up SQL scripts.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="migration">The migration.</param>
        /// <returns></returns>
        protected virtual IReadOnlyList<MigrationCommand> GenerateUpSql( EntityPlugin plugin, Migration migration )
        {
            var migrationId = migration.GetType().GetCustomAttribute<MigrationAttribute>()?.Id;
            var historyRow = new HistoryRow( migrationId, ProductInfo.GetVersion() );
            var historyScript = PluginHistoryRepository.GetInsertScript( plugin, historyRow );
            var historyCommand = RawSqlCommandBuilder.Build( historyScript );

            return MigrationsSqlGenerator
                .Generate( migration.UpOperations, migration.TargetModel )
                .Concat( new[] { new MigrationCommand( historyCommand, CurrentContext.Context, CommandLogger ) } )
                .ToList();
        }

        /// <summary>
        /// Generates down SQL scripts.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="migration">The migration.</param>
        /// <param name="previousMigration">The previous migration.</param>
        /// <returns></returns>
        protected virtual IReadOnlyList<MigrationCommand> GenerateDownSql( EntityPlugin plugin, Migration migration, Migration previousMigration )
        {
            var historyScript = PluginHistoryRepository.GetDeleteScript( plugin, migration.GetType().GetCustomAttribute<MigrationAttribute>().Id );
            var historyCommand = RawSqlCommandBuilder.Build( historyScript );

            return MigrationsSqlGenerator
                .Generate( migration.DownOperations, previousMigration?.TargetModel )
                .Concat( new[] { new MigrationCommand( historyCommand, CurrentContext.Context, CommandLogger ) } )
                .ToList();
        }

        #endregion

        #region Support Classes

        /// <summary>
        /// Helps with dependency tracking for migrations.
        /// </summary>
        protected class MigrationNode
        {
            #region Properties

            /// <summary>
            /// Gets the plugin for this node.
            /// </summary>
            public EntityPlugin Plugin { get; }

            /// <summary>
            /// Gets the migration identifier for this node.
            /// </summary>
            public string MigrationId { get; }

            /// <summary>
            /// Gets the migration for this node.
            /// </summary>
            public Migration Migration { get; }

            /// <summary>
            /// Gets the commands for this node.
            /// </summary>
            public IReadOnlyList<MigrationCommand> Commands { get; }

            /// <summary>
            /// Gets the dependencies for this node.
            /// </summary>
            public List<MigrationNode> Dependencies { get; }

            #endregion

            #region Constructors

            /// <summary>
            /// Creates a new instance of the <see cref="MigrationNode"/> class.
            /// </summary>
            /// <param name="plugin">The plugin for this node.</param>
            /// <param name="migration">The migration for this node.</param>
            /// <param name="commands">The commands for this node.</param>
            public MigrationNode( EntityPlugin plugin, Migration migration, IReadOnlyList<MigrationCommand> commands )
            {
                Plugin = plugin;
                Migration = migration;
                Commands = commands;
                MigrationId = migration.GetType().GetCustomAttribute<MigrationAttribute>().Id;
                Dependencies = new List<MigrationNode>();
            }

            #endregion
        }

        #endregion
    }
}
