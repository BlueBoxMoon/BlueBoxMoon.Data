using BlueBoxMoon.Data.EntityFramework;
using BlueBoxMoon.Data.EntityFramework.Migrations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Console.Runner
{
    [DbContext( typeof( DatabaseContext ) )]
    [PluginMigration( "1.0.0" )]
    public class PeopleMigration : EntityMigration
    {
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.EnsureSchema( typeof( Person ).GetSchemaNameForEntity() );

            migrationBuilder.CreateEntityTable(
                name: typeof( Person ).GetTableNameForEntity(),
                columns: table => new
                {
                    FirstName = table.Column<string>( nullable: false, maxLength: 100 ),
                    LastName = table.Column<string>( nullable: false, maxLength: 100 )
                },
                schema: typeof( Person  ).GetSchemaNameForEntity() );
        }

        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable(
                name: typeof( Person ).GetTableNameForEntity(),
                schema: typeof( Person ).GetSchemaNameForEntity() );
        }
    }
}
