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
        /// Create a table for a subclass of the <see cref="Entity"/> class.
        /// </summary>
        /// <typeparam name="TColumns"></typeparam>
        /// <param name="migrationBuilder"></param>
        /// <param name="name"></param>
        /// <param name="columns"></param>
        /// <param name="schema"></param>
        /// <param name="constraints"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static CreateTableBuilder<TColumns> CreateEntityTable<TColumns>( this MigrationBuilder migrationBuilder,
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
                    Id = table.Column<long>( nullable: false ).AutoIncrement( migrationBuilder ),
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
