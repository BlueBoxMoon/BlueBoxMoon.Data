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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using BlueBoxMoon.Data.EntityFramework.Migrations;
using BlueBoxMoon.Data.EntityFramework.Sqlite;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

namespace BlueBoxMoon.Data.EntityFramework.Tests
{
    public class MigrationTests
    {
        #region Setup/TearDown

        private SqliteConnection _connection;

        [SetUp]
        public void Setup()
        {
            _connection = new SqliteConnection( "DataSource=:memory:" );
            _connection.Open();
        }

        [TearDown]
        public void TearDown()
        {
            _connection.Close();
        }

        #endregion

        #region Tests

        /// <summary>
        /// Verifies that migrations are being executed in the correct order
        /// when a migration depends on another plugin.
        /// </summary>
        [Test]
        public void ValidDependencyOrder()
        {
            var expectedMigrations = new List<string>
            {
                "PluginC-1",
                "PluginC-2",
                "PluginB-1",
                "PluginA-1",
                "PluginA-2"
            };

            var serviceCollection = new ServiceCollection()
                .AddEntityDbContext<TestContext>( options =>
                {
                    options.UseSqlite( _connection );
                }, entityOptions =>
                {
                    entityOptions.UseSqlite();
                    entityOptions.WithPlugin<PluginA>();
                    entityOptions.WithPlugin<PluginB>();
                    entityOptions.WithPlugin<PluginC>();
                } );

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var ctx = serviceProvider.GetService<TestContext>();

            ctx.Database.MigratePlugins();

            var repo = ctx.GetService<IPluginHistoryRepository>();
            var migrations = repo.GetAppliedMigrations()
                .Select( a => $"{a.Plugin}-{a.MigrationId}" )
                .ToList();

            Assert.AreEqual( expectedMigrations, migrations );
        }

        /// <summary>
        /// Verifies that the <see cref="DependencyException"/> error is
        /// thrown when an unmet dependency exists.
        /// </summary>
        [Test]
        public void UnmetDependency()
        {
            var serviceCollection = new ServiceCollection()
                .AddEntityDbContext<TestContext>( options =>
                {
                    options.UseSqlite( _connection );
                }, entityOptions =>
                {
                    entityOptions.UseSqlite();
                    entityOptions.WithPlugin<PluginA>();
                    entityOptions.WithPlugin<PluginB>();
                } );

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var ctx = serviceProvider.GetService<TestContext>();

            Assert.Throws<DependencyException>( ctx.Database.MigratePlugins );
        }

        #endregion

        #region Support Classes

        private class TestContext : EntityDbContext
        {
            public TestContext( DbContextOptions options )
                : base( options )
            {
            }
        }

        private class PluginA : EntityPlugin
        {
            public override string Identifier => "PluginA";

            public override string Name => "PluginA";

            public override IEnumerable<Type> GetMigrations()
            {
                return new Type[] { typeof( Migration1), typeof( Migration2 ) };
            }

            [Migration( "1" )]
            [DependsOnPlugin( typeof( PluginB ), "1" )]
            public class Migration1 : EntityMigration
            {
                protected override void Up( [NotNull] MigrationBuilder migrationBuilder )
                {
                }
            }

            [Migration( "2" )]
            public class Migration2 : EntityMigration
            {
                protected override void Up( [NotNull] MigrationBuilder migrationBuilder )
                {
                }
            }
        }

        private class PluginB : EntityPlugin
        {
            public override string Identifier => "PluginB";

            public override string Name => "PluginB";

            public override IEnumerable<Type> GetMigrations()
            {
                return new Type[] { typeof( Migration1 ) };
            }

            [Migration( "1" )]
            [DependsOnPlugin( typeof( PluginC ), "2" )]
            public class Migration1 : EntityMigration
            {
                protected override void Up( [NotNull] MigrationBuilder migrationBuilder )
                {
                }
            }
        }

        private class PluginC : EntityPlugin
        {
            public override string Identifier => "PluginC";

            public override string Name => "PluginC";

            public override IEnumerable<Type> GetMigrations()
            {
                return new Type[] { typeof( Migration1 ), typeof( Migration2 ) };
            }

            [Migration( "1" )]
            public class Migration1 : EntityMigration
            {
                protected override void Up( [NotNull] MigrationBuilder migrationBuilder )
                {
                }
            }

            [Migration( "2" )]
            public class Migration2 : EntityMigration
            {
                protected override void Up( [NotNull] MigrationBuilder migrationBuilder )
                {
                }
            }
        }

        #endregion
    }
}