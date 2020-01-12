using System;

using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Extension methods for the CreateTableBuilder class.
    /// </summary>
    public static class CreateTableBuilderExtensions
    {
        /// <summary>
        /// Create a table for a subclass of the Model class.
        /// </summary>
        /// <typeparam name="TColumns"></typeparam>
        /// <param name="migrationBuilder"></param>
        /// <param name="name"></param>
        /// <param name="columns"></param>
        /// <param name="schema"></param>
        /// <param name="constraints"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static CreateTableBuilder<TColumns> CreateModelTable<TColumns>( this MigrationBuilder migrationBuilder,
            string name,
            Func<ColumnsBuilder, TColumns> columns,
            string schema = null,
            Action<CreateTableBuilder<TColumns>> constraints = null,
            string comment = null )
        {
            var createTableOperation = new CreateTableOperation
            {
                Schema = schema,
                Name = name,
                Comment = comment
            };

            createTableOperation.AppendColumns(
                table => new
                {
                    Id = table.Column<int>( nullable: false ).AutoIncrement( migrationBuilder ),
                    Guid = table.Column<Guid>( nullable: false )
                }, table =>
                {
                    table.PrimaryKey( $"PK_{createTableOperation.Name}", a => a.Id );
                    table.UniqueConstraint( $"IX_{createTableOperation.Name}_Guid", a => a.Guid );
                } );

            migrationBuilder.Operations.Add( createTableOperation );

            return createTableOperation.AppendColumns( columns, constraints );
        }
    }
}
