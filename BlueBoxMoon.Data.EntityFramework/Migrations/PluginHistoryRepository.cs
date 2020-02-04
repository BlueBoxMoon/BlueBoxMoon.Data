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
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlueBoxMoon.Data.EntityFramework.Migrations
{
    /// <summary>
    /// Abstract implementation of the <see cref="IPluginHistoryRepository"/> interface
    /// that provides database providers with a starting point to subclass from.
    /// </summary>
    /// <seealso cref="BlueBoxMoon.Data.EntityFramework.IPluginHistoryRepository" />
    public abstract class PluginHistoryRepository : IPluginHistoryRepository
    {
        #region Properties

        /// <summary>
        /// Gets the dependencies.
        /// </summary>
        /// <value>
        /// The dependencies.
        /// </value>
        protected PluginHistoryRepositoryDependencies Dependencies { get; }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        protected string TableName { get; } = "__EntityPluginMigrationsHistory";

        /// <summary>
        /// The model that identifies the structure of the table.
        /// </summary>
        private IModel _model;

        /// <summary>
        /// Gets the SQL statement that checks if the table exists.
        /// </summary>
        /// <value>
        /// The SQL statement that checks if the table exists.
        /// </value>
        protected abstract string ExistsSql { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginHistoryRepository"/> class.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        public PluginHistoryRepository( PluginHistoryRepositoryDependencies dependencies )
        {
            Dependencies = dependencies;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Ensures the model exists and returns it.
        /// </summary>
        /// <returns>The model that defines the structure of the history table.</returns>
        private IModel EnsureModel()
        {
            if ( _model == null )
            {
                var conventionSet = Dependencies.ConventionSetBuilder.CreateConventionSet();
                ConventionSet.Remove( conventionSet.ModelInitializedConventions, typeof( DbSetFindingConvention ) );

                var modelBuilder = new ModelBuilder( conventionSet );
                modelBuilder.Entity<PluginHistoryRow>(
                    table =>
                    {
                        table.ToTable( TableName );
                        table.HasKey( new[] { nameof( PluginHistoryRow.Plugin ), nameof( PluginHistoryRow.MigrationId ) } );
                        table.Property( h => h.Plugin ).HasMaxLength( 150 );
                        table.Property( h => h.MigrationId ).HasMaxLength( 150 );
                        table.Property( h => h.ProductVersion ).HasMaxLength( 32 ).IsRequired();
                    } );

                _model = modelBuilder.FinalizeModel();
            }

            return _model;
        }

        /// <summary>
        /// Gets the parameter object to be used in a database query.
        /// </summary>
        /// <returns>A <see cref="RelationalCommandParameterObject"/> instance.</returns>
        protected RelationalCommandParameterObject GetParameterObject()
        {
            return new RelationalCommandParameterObject(
                Dependencies.Connection,
                null,
                null,
                Dependencies.CurrentContext.Context,
                Dependencies.CommandLogger );
        }

        /// <summary>
        /// Checks for the existence of the repository.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the repository exists.
        /// </returns>
        public virtual bool Exists()
        {
            var result = Dependencies.RawSqlCommandBuilder
                .Build( ExistsSql )
                .ExecuteScalar( GetParameterObject() );

            return InterpretExistsResult( result );
        }

        /// <summary>
        /// Gets the SQL script to be executed in order to create
        /// the repository.
        /// </summary>
        /// <returns>
        /// A string containing the SQL statement.
        /// </returns>
        public virtual string GetCreateScript()
        {
            var model = EnsureModel();

            var operations = Dependencies.ModelDiffer.GetDifferences( null, model );
            var commandList = Dependencies.MigrationsSqlGenerator.Generate( operations, model );

            return string.Concat( commandList.Select( c => c.CommandText ) );
        }

        /// <summary>
        /// Interprets the result from the <see cref="ExistsSql"/> statement.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the statement indicates the table exists.</returns>
        protected abstract bool InterpretExistsResult( object value );

        /// <summary>
        /// Gets the SQL query to execute in order to find all the migrations
        /// related to the specified plugin identifier.
        /// </summary>
        /// <param name="pluginIdentifier">The plugin identifier.</param>
        /// <returns>A string containing the SQL statement.</returns>
        /// <remarks>The query should return two columns, MigrationId and ProductVersion.</remarks>
        protected virtual string GetAppliedMigrationsSql( string pluginIdentifier )
        {
            var stringTypeMapping = Dependencies.TypeMappingSource.GetMapping( typeof( string ) );

            var migrationIdColumn = Dependencies.SqlGenerationHelper
                .DelimitIdentifier( nameof( PluginHistoryRow.MigrationId ) );

            var productVersionColumn = Dependencies.SqlGenerationHelper
                .DelimitIdentifier( nameof( PluginHistoryRow.ProductVersion ) );

            var pluginColumn = Dependencies.SqlGenerationHelper
                .DelimitIdentifier( nameof( PluginHistoryRow.Plugin ) );

            var tableName = Dependencies.SqlGenerationHelper
                .DelimitIdentifier( TableName );

            pluginIdentifier = stringTypeMapping.GenerateSqlLiteral( pluginIdentifier );

            return $"SELECT {migrationIdColumn}, {productVersionColumn} FROM {tableName} WHERE {pluginColumn} = {pluginIdentifier} ORDER BY {migrationIdColumn}";
        }

        /// <summary>
        /// Gets the applied migrations for the specified plugin.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <returns>
        /// A read only list of <see cref="HistoryRow" /> objects that represent which migrations have been run.
        /// </returns>
        public IReadOnlyList<HistoryRow> GetAppliedMigrations( EntityPlugin plugin )
        {
            var rows = new List<HistoryRow>();

            if ( Exists() )
            {
                var command = Dependencies.RawSqlCommandBuilder
                    .Build( GetAppliedMigrationsSql( plugin.Identifier ) );

                using ( var reader = command.ExecuteReader( GetParameterObject() ) )
                {
                    while ( reader.Read() )
                    {
                        rows.Add( new HistoryRow( reader.DbDataReader.GetString( 0 ), reader.DbDataReader.GetString( 1 ) ) );
                    }
                }
            }

            return rows;
        }

        /// <summary>
        /// Gets the SQL script to be executed in order to save the <see cref="HistoryRow" />
        /// to the database.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="historyRow">The history row.</param>
        /// <returns>
        /// A string containg the SQL statement.
        /// </returns>
        public virtual string GetInsertScript( EntityPlugin plugin, HistoryRow historyRow )
        {
            var stringTypeMapping = Dependencies.TypeMappingSource.GetMapping( typeof( string ) );

            var migrationIdColumn = Dependencies.SqlGenerationHelper
                .DelimitIdentifier( nameof( PluginHistoryRow.MigrationId ) );

            var productVersionColumn = Dependencies.SqlGenerationHelper
                .DelimitIdentifier( nameof( PluginHistoryRow.ProductVersion ) );

            var pluginColumn = Dependencies.SqlGenerationHelper
                .DelimitIdentifier( nameof( PluginHistoryRow.Plugin ) );

            var tableName = Dependencies.SqlGenerationHelper
                .DelimitIdentifier( TableName );

            var pluginName = stringTypeMapping.GenerateSqlLiteral( plugin.Identifier );
            var migrationId = stringTypeMapping.GenerateSqlLiteral( historyRow.MigrationId );
            var productVersion = stringTypeMapping.GenerateSqlLiteral( historyRow.ProductVersion );

            return $"INSERT INTO {tableName} ({pluginColumn}, {migrationIdColumn}, {productVersionColumn}) VALUES ({pluginName}, {migrationId}, {productVersion})";
        }

        /// <summary>
        /// Gets the SQL script to be executed in order to delete the
        /// specified migration's <see cref="HistoryRow" /> from the database.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="migrationId">The migration identifier.</param>
        /// <returns>
        /// A string containing the SQL statement.
        /// </returns>
        public string GetDeleteScript( EntityPlugin plugin, string migrationId )
        {
            var stringTypeMapping = Dependencies.TypeMappingSource.GetMapping( typeof( string ) );

            var migrationIdColumn = Dependencies.SqlGenerationHelper
                .DelimitIdentifier( nameof( PluginHistoryRow.MigrationId ) );

            var pluginColumn = Dependencies.SqlGenerationHelper
                .DelimitIdentifier( nameof( PluginHistoryRow.Plugin ) );

            var tableName = Dependencies.SqlGenerationHelper
                .DelimitIdentifier( TableName );

            var pluginName = stringTypeMapping.GenerateSqlLiteral( plugin.Identifier );
            migrationId = stringTypeMapping.GenerateSqlLiteral( migrationId );

            return $"DELETE FROM {tableName} WHERE {pluginColumn} = {pluginName} AND {migrationIdColumn} = {migrationId}";
        }

        #endregion
    }
}
