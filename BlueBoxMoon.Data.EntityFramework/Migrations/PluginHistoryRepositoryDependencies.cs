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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlueBoxMoon.Data.EntityFramework.Migrations
{
    /// <summary>
    /// This is a utility class that simplifies the required work of
    /// concrete subclasses to <see cref="PluginHistoryRepository"/>.
    /// </summary>
    public class PluginHistoryRepositoryDependencies
    {
        #region Properties

        /// <summary>
        /// Gets the raw SQL command builder.
        /// </summary>
        /// <value>
        /// The raw SQL command builder.
        /// </value>
        public IRawSqlCommandBuilder RawSqlCommandBuilder { get; }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public IRelationalConnection Connection { get; }

        /// <summary>
        /// Gets the current context.
        /// </summary>
        /// <value>
        /// The current context.
        /// </value>
        public ICurrentDbContext CurrentContext { get; }

        /// <summary>
        /// Gets the convention set builder.
        /// </summary>
        /// <value>
        /// The convention set builder.
        /// </value>
        public IConventionSetBuilder ConventionSetBuilder { get; }

        /// <summary>
        /// Gets the model differ.
        /// </summary>
        /// <value>
        /// The model differ.
        /// </value>
        public IMigrationsModelDiffer ModelDiffer { get; }

        /// <summary>
        /// Gets the migrations SQL generator.
        /// </summary>
        /// <value>
        /// The migrations SQL generator.
        /// </value>
        public IMigrationsSqlGenerator MigrationsSqlGenerator { get; }

        /// <summary>
        /// Gets the SQL generation helper.
        /// </summary>
        /// <value>
        /// The SQL generation helper.
        /// </value>
        public ISqlGenerationHelper SqlGenerationHelper { get; }

        /// <summary>
        /// Gets the type mapping source.
        /// </summary>
        /// <value>
        /// The type mapping source.
        /// </value>
        public IRelationalTypeMappingSource TypeMappingSource { get; }

        /// <summary>
        /// Gets the command logger.
        /// </summary>
        /// <value>
        /// The command logger.
        /// </value>
        public IDiagnosticsLogger<DbLoggerCategory.Database.Command> CommandLogger { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginHistoryRepositoryDependencies"/> class.
        /// </summary>
        /// <param name="rawSqlCommandBuilder">The raw SQL command builder.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="currentContext">The current context.</param>
        /// <param name="conventionSetBuilder">The convention set builder.</param>
        /// <param name="modelDiffer">The model differ.</param>
        /// <param name="migrationsSqlGenerator">The migrations SQL generator.</param>
        /// <param name="sqlGenerationHelper">The SQL generation helper.</param>
        /// <param name="typeMappingSource">The type mapping source.</param>
        /// <param name="commandLogger">The command logger.</param>
        public PluginHistoryRepositoryDependencies(
            IRawSqlCommandBuilder rawSqlCommandBuilder,
            IRelationalConnection connection,
            ICurrentDbContext currentContext,
            IConventionSetBuilder conventionSetBuilder,
            IMigrationsModelDiffer modelDiffer,
            IMigrationsSqlGenerator migrationsSqlGenerator,
            ISqlGenerationHelper sqlGenerationHelper,
            IRelationalTypeMappingSource typeMappingSource,
            IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger )
        {
            RawSqlCommandBuilder = rawSqlCommandBuilder;
            Connection = connection;
            CurrentContext = currentContext;
            ConventionSetBuilder = conventionSetBuilder;
            ModelDiffer = modelDiffer;
            MigrationsSqlGenerator = migrationsSqlGenerator;
            SqlGenerationHelper = sqlGenerationHelper;
            TypeMappingSource = typeMappingSource;
            CommandLogger = commandLogger;
        }

        #endregion
    }

}
