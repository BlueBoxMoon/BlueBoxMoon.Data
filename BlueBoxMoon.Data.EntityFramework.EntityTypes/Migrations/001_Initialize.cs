﻿// MIT License
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

using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueBoxMoon.Data.EntityFramework.EntityTypes.Migrations
{
    [PluginMigration( "0.5.0" )]
    public class Initialize : EntityMigration
    {
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.EnsureSchema( typeof( EntityType ).GetSchemaNameForEntity() );

            migrationBuilder.CreateEntityTable( typeof( EntityType ).GetTableNameForEntity(),
                table => new
                {
                    Name = table.Column<string>( nullable: false, maxLength: 100 ),
                    QualifiedName = table.Column<string>( nullable: false, maxLength: 250 )
                },
                typeof( EntityType ).GetSchemaNameForEntity(),
                table =>
                {
                    table.UniqueConstraint( "IX_EntityTypes_Name", a => a.Name );
                    table.UniqueConstraint( "IX_EntityTypes_QualifiedName", a => a.QualifiedName );
                } );
        }

        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable( typeof( EntityType ).GetTableNameForEntity(), typeof( EntityType ).GetSchemaNameForEntity() );
        }
    }
}
