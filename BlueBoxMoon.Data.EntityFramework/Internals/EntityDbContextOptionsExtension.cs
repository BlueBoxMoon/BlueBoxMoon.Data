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

using BlueBoxMoon.Data.EntityFramework.Infrastructure;
using BlueBoxMoon.Data.EntityFramework.Migrations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BlueBoxMoon.Data.EntityFramework.Internals
{
    /// <summary>
    /// Defines the context options extension that will be used by the
    /// <see cref="EntityDbContext"/> class.
    /// </summary>
    internal class EntityDbContextOptionsExtension : IDbContextOptionsExtension
    {
        #region Properties

        /// <summary>
        /// Information/metadata about the extension.
        /// </summary>
        public DbContextOptionsExtensionInfo Info
        {
            get
            {
                if ( _info == null )
                {
                    _info = new ExtensionInfo( this );
                }

                return _info;
            }
        }
        private DbContextOptionsExtensionInfo _info;

        /// <summary>
        /// The builder that will generate the options.
        /// </summary>
        public EntityDbContextOptionsBuilder Builder { get; }

        /// <summary>
        /// The application (parent) service collection.
        /// </summary>
        internal IServiceCollection ApplicationServiceCollection { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="EntityDbContextOptionsExtension"/> class.
        /// </summary>
        /// <param name="baseOptionsBuilder">The base options builder.</param>
        public EntityDbContextOptionsExtension( DbContextOptionsBuilder baseOptionsBuilder, IServiceCollection applicationServiceCollection )
        {
            ApplicationServiceCollection = applicationServiceCollection;
            Builder = new EntityDbContextOptionsBuilder( baseOptionsBuilder, applicationServiceCollection.BuildServiceProvider() );
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the services required to make the selected options work. This is used when there
        /// is no external <see cref="T:System.IServiceProvider" /> and EF is maintaining its own service
        /// provider internally. This allows database providers (and other extensions) to register their
        /// required services when EF is creating an service provider.
        /// </summary>
        /// <param name="services"> The collection to add services to. </param>
        public void ApplyServices( IServiceCollection services )
        {
            //
            // Add a way for classes locked inside the DbContext scope to find
            // the parent provider.
            //
            services.AddSingleton<IParentServiceProvider>( serviceProvider =>
            {
                return new ParentServiceProvider( ApplicationServiceCollection.BuildServiceProvider() );
            } );

            services.AddScoped<IPluginMigrator, PluginMigrator>()
                .AddScoped<PluginHistoryRepositoryDependencies>()
                .AddSingleton( typeof( IEntityDatabaseFeatures ), Builder.Options.Provider.Features );

            var contextType = Builder.BaseOptionsBuilder.Options.ContextType;
            services.AddSingleton( typeof( IDbContextFactory<> ).MakeGenericType( contextType ), typeof( DbContextFactory<> ).MakeGenericType( contextType ) );

            //
            // Let plugins add additional services.
            //
            foreach ( var plugin in Builder.Options.Plugins )
            {
                plugin.ApplyServices( services, Builder.Options );
            }

            foreach ( var action in Builder.ApplyServiceActions )
            {
                action( services );
            }
        }

        /// <summary>
        /// Gives the extension a chance to validate that all options in the extension are valid.
        /// Most extensions do not have invalid combinations and so this will be a no-op.
        /// If options are invalid, then an exception should be thrown.
        /// </summary>
        /// <param name="options"> The options being validated. </param>
        public void Validate( IDbContextOptions options )
        {
            if ( Builder.Options.Provider == null )
            {
                throw new Exception( "Database provider has not been fully configured." );
            }
        }

        #endregion

        #region Support Classes

        /// <summary>
        /// Defines the extension information for <see cref="EntityDbContextOptionsExtension"/> class.
        /// </summary>
        private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
        {
            public ExtensionInfo( IDbContextOptionsExtension extension )
                : base( extension )
            {
            }

            public new EntityDbContextOptionsExtension Extension
                => ( EntityDbContextOptionsExtension ) base.Extension;

            public override bool IsDatabaseProvider => false;

            public override string LogFragment => string.Empty;

            public override void PopulateDebugInfo( IDictionary<string, string> debugInfo )
            {
            }

            public override long GetServiceProviderHashCode() => 0;
        }

        #endregion
    }
}
