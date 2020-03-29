using System;
using System.Collections.Generic;
using System.Linq;

using BlueBoxMoon.Data.EntityFramework;
using BlueBoxMoon.Data.EntityFramework.Facets;
using BlueBoxMoon.Data.EntityFramework.Cache;
using BlueBoxMoon.Data.EntityFramework.EntityTypes;
using BlueBoxMoon.Data.EntityFramework.Infrastructure;
using BlueBoxMoon.Data.EntityFramework.Migrations;
using BlueBoxMoon.Data.EntityFramework.Sqlite;
using BlueBoxMoon.Data.EntityFramework.PostgreSQL;

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
                .AddSingleton<IJsonSerializer, JsonSerializer>()
                .AddEntityDbContext<DatabaseContext>( options =>
                {
                    options.UseSqlite( "Data Source=database.db" );
                    options.EnableSensitiveDataLogging();
                }, entityOptions =>
                {
                    entityOptions.UseSqlite();
                    entityOptions.WithPlugin<TestPlugin>();
                    entityOptions.WithEntity<Person, PersonDataSet>();
                    entityOptions.UseEntityCache( cacheOptions =>
                    {
                        cacheOptions.WithCachedType<Person, CachedPerson>();
                    } );
                    entityOptions.UseEntityTypes();
                    entityOptions.UseFacets();
                } );

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var ctx = serviceProvider.GetService<DatabaseContext>();
            ctx.Database.Migrate();

            ctx.Database.MigratePlugins();
            ctx.Database.InitializePlugins();

            var peopleSet = ctx.GetDataSet<Person>();
            var c1 = ctx.GetCachedDataSet<CachedPerson>().GetById( 1 );
            var p1 = new Person { FirstName = "Daniel", LastName = Guid.NewGuid().ToString() };
            peopleSet.Add( p1 );
            ctx.SaveChanges();
            var c2 = ctx.GetCachedDataSet<CachedPerson>().GetById( 1 );

            var ctxFactory = serviceProvider.GetService<IDbContextFactory<DatabaseContext>>();

            using ( var ctx2 = ctxFactory.CreateContext() )
            {
                var attrSet = ctx2.GetDataSet<Facet>();
                if ( attrSet.GetById( 1 ) == null )
                {
                    var attr = attrSet.Create();
                    attr.EntityTypeId = ctx2.GetCachedDataSet<CachedEntityType>().GetByType<Person>().Id;
                    attr.Name = "Gender";
                    attr.Key = "Gender";
                    attr.Description = "The gender of the person.";
                    attr.DefaultValue = "Unknown";
                    attr.SetQualifiers<Person>( a => a.FirstName == "Daniel" );
                    attrSet.Add( attr );
                    ctx2.SaveChanges();
                }

                var p2 = ctx2.GetDataSet<Person>().GetById( 1 );
                p2.LoadFacetValues( ctx2 );
                var v = p2.GetFacetValue( "Gender" );
                p2.SetFacetValue( "Gender", "Male" );
                p2.SaveFacetValues( ctx2 );
            }

            using ( var ctx2 = ctxFactory.CreateContext() )
            {
                var d = new DateTimeOffset( new DateTime( 2020, 3, 29, 11, 4, 0, DateTimeKind.Local ) );
                var x = ctx2.GetDataSet<Person>().Where( a => a.CreatedDateTime >= d ).ToList();
                var y = ctx2.GetDataSet<Person>().Where( a => a.CreatedDateTime < d ).ToList();
                var p2 = ctx2.GetDataSet<Person>().GetById( 1 );
                p2.LastName = Guid.NewGuid().ToString();
                ctx2.SaveChanges();
                var cachedSet = ctx2.GetCachedDataSet<CachedPerson>();
                var c3 = cachedSet.GetById( 1 );
                var list = ctx2.GetDataSet<Person>().ToList();
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

    public class JsonSerializer : IJsonSerializer
    {
        public T Deserialize<T>( string json )
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>( json );
        }

        public string Serialize( object value )
        {
            return System.Text.Json.JsonSerializer.Serialize( value );
        }
    }
}
