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
using Microsoft.Extensions.DependencyInjection;

namespace BlueBoxMoon.Data.EntityFramework.Sqlite
{
    public static class EntityDbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Configures the entity database to use the Sqlite provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static EntityDbContextOptionsBuilder UseSqlite( this EntityDbContextOptionsBuilder optionsBuilder )
        {
            //optionsBuilder.BaseOptionsBuilder.ReplaceService<IMigrationsSqlGenerator, SqliteEntityMigrationsSqlGenerator>();
            optionsBuilder.BaseOptionsBuilder.ReplaceService<ISqlGenerationHelper, SqliteEntitySqlGenerationHelper>();

            return optionsBuilder.UseDatabaseProviderFeatures<SqliteEntityDatabaseFeatures>()
                .ApplyServices( services =>
                {
                    services.AddScoped<IPluginHistoryRepository, SqlitePluginHistoryRepository>();
                } );
        }
    }

    //public class SqliteEntityMigrationsSqlGenerator : SqliteMigrationsSqlGenerator
    //{
    //    public SqliteEntityMigrationsSqlGenerator( MigrationsSqlGeneratorDependencies dependencies, IMigrationsAnnotationProvider migrationsAnnotations )
    //        : base( dependencies, migrationsAnnotations )
    //    {
    //    }

    //    public override IReadOnlyList<MigrationCommand> Generate( IReadOnlyList<MigrationOperation> operations, IModel model = null )
    //    {
    //        foreach ( var operation in operations )
    //        {
    //            //if ( operation is CreateTableOperation createTableOperation && createTableOperation.Schema != null )
    //            //{
    //            //    createTableOperation.Name = $"{createTableOperation.Schema}_{createTableOperation.Name}";
    //            //}
    //        }
    //        // AddColumnOperation
    //        // AddForeignKeyOperation
    //        // AddPrimaryKeyOperation
    //        // AddUniqueConstraintOperation
    //        // AlterColumnOperation
    //        // AlterSequenceOperation
    //        // AlterTableOperation
    //        // CreateCheckConstraintOperation
    //        // CreateIndexOperation
    //        // CreateSequenceOperation
    //        // DeleteDataOperation
    //        // DropCheckConstraintOperation
    //        // DropColumnOperation
    //        // DropForeignKeyOperation
    //        // DropIndexOperation
    //        // DropPrimaryKeyOperation
    //        // DropSequenceOperation
    //        // DropTableOperation
    //        // DropUniqueConstraintOperation
    //        // InsertDataOperation
    //        // RenameColumnOperation
    //        // RenameIndexOperation
    //        // RenameSequenceOperation
    //        // RenameTableOperation
    //        // RestartSequenceOperation
    //        // UpdateDataOperation

    //        return base.Generate( operations, model );
    //    }
    //}
}
