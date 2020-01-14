using System;
using System.Collections.Generic;
using System.Linq;

using BlueBoxMoon.Data.EntityFramework;
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
                .AddDbContext<DatabaseContext>( options =>
                {
                    options.UseSqlite( "Data Source=database.db" );
                    options.UseEntityDbContext( o =>
                    {
                        o.UseSqlite();
                        o.RegisterEntity<Person, PersonDataSet>();
                    } );
                } )
                .AddSingleton<IDbContextFactory<DatabaseContext>, DbContextFactory<DatabaseContext>>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var ctx = serviceProvider.GetService<DatabaseContext>();
            ctx.Database.Migrate();

            var plugin = new EntityPlugin( "com.blueboxmoon.test", new List<Type> { typeof( TestMigration ) } );
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

    [Migration( "20200112_Initial" )]
    public class TestMigration : EntityMigration
    {
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.CreateEntityTable( "TestPlugin",
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
