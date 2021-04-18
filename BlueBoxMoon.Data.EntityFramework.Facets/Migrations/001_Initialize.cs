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

using BlueBoxMoon.Data.EntityFramework.EntityTypes;
using BlueBoxMoon.Data.EntityFramework.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueBoxMoon.Data.EntityFramework.Facets.Migrations
{
    [PluginMigration( "0.5.0", 1 )]
    [DependsOnPlugin( typeof( EntityTypesPlugin ), "0.5.0" )]
    public class Initialize : EntityMigration
    {
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.EnsureSchema( typeof( Facet ).GetSchemaNameForEntity() );

            //
            // Create the Facet table.
            //
            migrationBuilder.CreateEntityTable(
                name: typeof( Facet ).GetTableNameForEntity(),
                columns: table => new
                {
                    EntityTypeId = table.Column<long>( nullable: false ),
                    Name = table.Column<string>( nullable: false, maxLength: 100 ),
                    Key = table.Column<string>( nullable: false, maxLength: 50 ),
                    Description = table.Column<string>( nullable: true ),
                    Qualifiers = table.Column<string>( nullable: true ),
                    DefaultValue = table.Column<string>( nullable: false )
                },
                schema: typeof( Facet ).GetSchemaNameForEntity(),
                constraints: table =>
                {
                    table.ForeignKey(
                        name: $"FK_{typeof( Facet ).GetTableNameForEntity()}_{nameof( Facet.EntityTypeId )}",
                        column: a => a.EntityTypeId,
                        principalTable: typeof( EntityType ).GetTableNameForEntity(),
                        principalColumn: nameof( EntityType.Id ),
                        principalSchema: typeof( EntityType ).GetSchemaNameForEntity(),
                        onDelete: ReferentialAction.Cascade );
                } );

            //
            // Create the FacetValue table.
            //
            migrationBuilder.CreateEntityTable(
                name: typeof( FacetValue ).GetTableNameForEntity(),
                columns: table => new
                {
                    FacetId = table.Column<long>( nullable: false ),
                    EntityId = table.Column<long>( nullable: false ),
                    Value = table.Column<string>( nullable: true )
                },
                schema: typeof( FacetValue ).GetSchemaNameForEntity(),
                constraints: table =>
                {
                    table.ForeignKey(
                        name: $"FK_{typeof( FacetValue ).GetTableNameForEntity()}_{nameof( FacetValue.FacetId )}",
                        column: a => a.FacetId,
                        principalTable: typeof( Facet ).GetTableNameForEntity(),
                        principalColumn: nameof( Facet.Id ),
                        principalSchema: typeof( Facet ).GetSchemaNameForEntity(),
                        onDelete: ReferentialAction.Cascade );
                } );
        }

        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable(
                name: typeof( FacetValue ).GetTableNameForEntity(),
                schema: typeof( FacetValue ).GetSchemaNameForEntity() );

            migrationBuilder.DropTable(
                name: typeof( Facet ).GetTableNameForEntity(),
                schema: typeof( Facet ).GetSchemaNameForEntity() );
        }
    }
}
