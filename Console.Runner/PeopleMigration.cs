using BlueBoxMoon.Data.EntityFramework;
using BlueBoxMoon.Data.EntityFramework.Migrations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Console.Runner
{
    [DbContext( typeof( DatabaseContext ) )]
    [Migration( "1" )]
    public class PeopleMigration : EntityMigration
    {
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.CreateEntityTable( "People",
                table => new
                {
                    FirstName = table.Column<string>( nullable: false, maxLength: 100 ),
                    LastName = table.Column<string>( nullable: false, maxLength: 100 )
                }, "testSchema" );
        }

        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable( "People" );
        }
    }
}
