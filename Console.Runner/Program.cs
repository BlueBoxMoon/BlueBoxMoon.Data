using System;
using System.Collections.Generic;
using System.Linq;

using BlueBoxMoon.Data.EntityFramework;
using BlueBoxMoon.Data.EntityFramework.Infrastructure;
using BlueBoxMoon.Data.EntityFramework.Migrations;
using BlueBoxMoon.Data.EntityFramework.Sqlite;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Console.Runner
{
    class Program
    {
        static void Main( string[] args )
        {
            var serviceCollection = new ServiceCollection()
                .AddLogging( a => a.AddConsole() )
                .AddEntityDbContext<DatabaseContext>( options =>
                {
                    options.UseSqlite( "Data Source=database.db" );
                }, entityOptions =>
                {
                    entityOptions.UseSqlite();
                    entityOptions.RegisterEntity<Person, PersonDataSet>();
                } );

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var ctx = serviceProvider.GetService<DatabaseContext>();
            ctx.Database.Migrate();

            var plugin = new TestPlugin();
            ctx.Database.MigratePlugin( plugin );

            var peopleSet = ctx.GetDataSet<Person>();
            peopleSet.Add( new Person { FirstName = "Daniel", LastName = Guid.NewGuid().ToString() } );
            ctx.SaveChanges();

            var ctxFactory = serviceProvider.GetService<IDbContextFactory<DatabaseContext>>();
            using ( var ctx2 = ctxFactory.CreateContext() )
            {
                var list = peopleSet.ToList();
            }
        }
    }

    public class TestPlugin : EntityPlugin
    {
        public override IEnumerable<Type> GetMigrations()
        {
            return new List<Type> { typeof( TestMigration ) };
        }
    }

    [Migration( "20200112_Initial" )]
    public class TestMigration : EntityMigration
    {
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.CreateEntityTable( "com_blueboxmoon_TestPlugin",
                table => new
                {
                    Value = table.Column<string>()
                } );
        }

        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable( "TestPlugin" );
        }
    }
}
