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

namespace BlueBoxMoon.Data.EntityFramework.Tests.Core
{
    public class EntityDatabaseFacadeExtensionsTests
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

        [Test]
        public void IsMigrationUpCalledWhenInstallingPlugin()
        {
            bool migrationUpCalled = true;

            var callbacks = new TestCallbacks
            {
                MigrationUp = () => migrationUpCalled = true
            };

            var serviceCollection = new ServiceCollection()
                .AddEntityDbContext<TestContext>( options =>
                {
                    options.UseSqlite( _connection );
                }, entityOptions =>
                {
                    entityOptions.UseSqlite();
                    entityOptions.WithPlugin<TestPlugin>();
                    entityOptions.ApplyServices( sc => sc.AddSingleton( callbacks ) );
                } )
                .AddSingleton( callbacks );

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var ctx = serviceProvider.GetService<TestContext>();

            ctx.Database.InstallPlugin( typeof( TestPlugin ) );

            Assert.AreEqual( true, migrationUpCalled );
        }

        [Test]
        public void IsMigrationUpCalledWhenInstallingPluginByGeneric()
        {
            bool migrationUpCalled = true;

            var callbacks = new TestCallbacks
            {
                MigrationUp = () => migrationUpCalled = true
            };

            var serviceCollection = new ServiceCollection()
                .AddEntityDbContext<TestContext>( options =>
                {
                    options.UseSqlite( _connection );
                }, entityOptions =>
                {
                    entityOptions.UseSqlite();
                    entityOptions.WithPlugin<TestPlugin>();
                    entityOptions.ApplyServices( sc => sc.AddSingleton( callbacks ) );
                } )
                .AddSingleton( callbacks );

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var ctx = serviceProvider.GetService<TestContext>();

            ctx.Database.InstallPlugin<TestPlugin>();

            Assert.AreEqual( true, migrationUpCalled );
        }

        [Test]
        public void IsMigrationDownCalledWhenRemovingPlugin()
        {
            bool migrationDownCalled = true;

            var callbacks = new TestCallbacks
            {
                MigrationDown = () => migrationDownCalled = true
            };

            var serviceCollection = new ServiceCollection()
                .AddEntityDbContext<TestContext>( options =>
                {
                    options.UseSqlite( _connection );
                }, entityOptions =>
                {
                    entityOptions.UseSqlite();
                    entityOptions.WithPlugin<TestPlugin>();
                    entityOptions.ApplyServices( sc => sc.AddSingleton( callbacks ) );
                } )
                .AddSingleton( callbacks );

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var ctx = serviceProvider.GetService<TestContext>();

            ctx.Database.InstallPlugin( typeof( TestPlugin ) );
            ctx.Database.RemovePlugin( typeof( TestPlugin ) );

            Assert.AreEqual( true, migrationDownCalled );
        }

        [Test]
        public void IsMigrationDownCalledWhenRemovingPluginByGeneric()
        {
            bool migrationDownCalled = true;

            var callbacks = new TestCallbacks
            {
                MigrationDown = () => migrationDownCalled = true
            };

            var serviceCollection = new ServiceCollection()
                .AddEntityDbContext<TestContext>( options =>
                {
                    options.UseSqlite( _connection );
                }, entityOptions =>
                {
                    entityOptions.UseSqlite();
                    entityOptions.WithPlugin<TestPlugin>();
                    entityOptions.ApplyServices( sc => sc.AddSingleton( callbacks ) );
                } )
                .AddSingleton( callbacks );

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var ctx = serviceProvider.GetService<TestContext>();

            ctx.Database.InstallPlugin<TestPlugin>();
            ctx.Database.RemovePlugin<TestPlugin>();

            Assert.AreEqual( true, migrationDownCalled );
        }

        [Test]
        public void IsInitializeCalledWhenInitializingPlugin()
        {
            bool initializeCalled = true;

            var callbacks = new TestCallbacks
            {
                PluginIntialized = () => initializeCalled = true
            };

            var serviceCollection = new ServiceCollection()
                .AddEntityDbContext<TestContext>( options =>
                {
                    options.UseSqlite( _connection );
                }, entityOptions =>
                {
                    entityOptions.UseSqlite();
                    entityOptions.WithPlugin<TestPlugin>();
                    entityOptions.ApplyServices( sc => sc.AddSingleton( callbacks ) );
                } )
                .AddSingleton( callbacks );

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var ctx = serviceProvider.GetService<TestContext>();

            ctx.Database.InitializePlugin( typeof( TestPlugin ) );

            Assert.AreEqual( true, initializeCalled );
        }

        [Test]
        public void IsInitializeCalledWhenInitializingPluginByGeneric()
        {
            bool initializeCalled = true;

            var callbacks = new TestCallbacks
            {
                PluginIntialized = () => initializeCalled = true
            };

            var serviceCollection = new ServiceCollection()
                .AddEntityDbContext<TestContext>( options =>
                {
                    options.UseSqlite( _connection );
                }, entityOptions =>
                {
                    entityOptions.UseSqlite();
                    entityOptions.WithPlugin<TestPlugin>();
                    entityOptions.ApplyServices( sc => sc.AddSingleton( callbacks ) );
                } )
                .AddSingleton( callbacks );

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var ctx = serviceProvider.GetService<TestContext>();

            ctx.Database.InitializePlugin<TestPlugin>();

            Assert.AreEqual( true, initializeCalled );
        }

        [Test]
        public void IsInitializeCalledWhenInitializingPlugins()
        {
            bool initializeCalled = true;

            var callbacks = new TestCallbacks
            {
                PluginIntialized = () => initializeCalled = true
            };

            var serviceCollection = new ServiceCollection()
                .AddEntityDbContext<TestContext>( options =>
                {
                    options.UseSqlite( _connection );
                }, entityOptions =>
                {
                    entityOptions.UseSqlite();
                    entityOptions.WithPlugin<TestPlugin>();
                    entityOptions.ApplyServices( sc => sc.AddSingleton( callbacks ) );
                } )
                .AddSingleton( callbacks );

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var ctx = serviceProvider.GetService<TestContext>();

            //
            // Really we need to test two plugins, but work with what we got for now.
            //
            ctx.Database.InitializePlugins();

            Assert.AreEqual( true, initializeCalled );
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

        private class TestCallbacks
        {
            public Action PluginIntialized { get; set; }

            public Action MigrationUp { get; set; }

            public Action MigrationDown { get; set; }
        }

        private class TestPlugin : EntityPlugin
        {
            public override string Identifier => "TestPlugin";

            public override string Name => "TestPlugin";

            private readonly TestCallbacks _callbacks;

            public TestPlugin( TestCallbacks callbacks )
            {
                _callbacks = callbacks;
            }

            public override void Initialize( EntityDbContext context )
            {
                base.Initialize( context );

                _callbacks?.PluginIntialized?.Invoke();
            }

            public override IEnumerable<Type> GetMigrations()
            {
                return new Type[] { typeof( Migration1 ) };
            }

            [PluginMigration( "1.0.0" )]
            public class Migration1 : EntityMigration
            {
                private readonly TestCallbacks _callbacks;

                public Migration1( TestCallbacks callbacks )
                {
                    _callbacks = callbacks;
                }

                protected override void Up( [NotNull] MigrationBuilder migrationBuilder )
                {
                    _callbacks?.MigrationUp?.Invoke();
                }

                protected override void Down( MigrationBuilder migrationBuilder )
                {
                    _callbacks?.MigrationDown?.Invoke();
                }
            }
        }

        #endregion
    }
}
