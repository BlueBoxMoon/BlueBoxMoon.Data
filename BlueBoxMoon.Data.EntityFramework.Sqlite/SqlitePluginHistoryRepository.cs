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
using BlueBoxMoon.Data.EntityFramework.Migrations;

using Microsoft.EntityFrameworkCore.Storage;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// The Sqlite <see cref="PluginHistoryRepository"/> implementation.
    /// </summary>
    /// <seealso cref="BlueBoxMoon.Data.EntityFramework.Migrations.PluginHistoryRepository" />
    public class SqlitePluginHistoryRepository : PluginHistoryRepository
    {
        #region Properties

        /// <summary>
        /// Gets the SQL statement that checks if the table exists.
        /// </summary>
        /// <value>
        /// The SQL statement that checks if the table exists.
        /// </value>
        protected override string ExistsSql
        {
            get
            {
                var stringTypeMapping = Dependencies.TypeMappingSource.GetMapping( typeof( string ) );

                var sqliteMaster = Dependencies.SqlGenerationHelper
                    .DelimitIdentifier( "sqlite_master" );
                
                var nameColumn = Dependencies.SqlGenerationHelper
                    .DelimitIdentifier( "name" );

                var typeColumn = Dependencies.SqlGenerationHelper
                    .DelimitIdentifier( "type" );

                var tableName = stringTypeMapping.GenerateSqlLiteral( TableName );

                return $"SELECT COUNT(*) FROM {sqliteMaster} WHERE {nameColumn} = {tableName} AND {typeColumn} = 'table';";
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlitePluginHistoryRepository"/> class.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        public SqlitePluginHistoryRepository( PluginHistoryRepositoryDependencies dependencies )
            : base( dependencies )
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Interprets the result from the <see cref="ExistsSql" /> statement.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the statement indicates the table exists.
        /// </returns>
        protected override bool InterpretExistsResult( object value )
        {
            return ( long ) value != 0;
        }

        #endregion
    }

}
