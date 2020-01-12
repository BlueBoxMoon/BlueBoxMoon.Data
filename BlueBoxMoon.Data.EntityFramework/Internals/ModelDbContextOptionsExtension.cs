using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BlueBoxMoon.Data.EntityFramework.Internals
{
    /// <summary>
    /// Defines the context options extension that will be used by the
    /// <see cref="ModelDbContext"/> class.
    /// </summary>
    internal class ModelDbContextOptionsExtension : IDbContextOptionsExtension
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
        public ModelDbContextOptionsBuilder Builder { get; } = new ModelDbContextOptionsBuilder();

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
            services.AddSingleton( serviceProvider =>
            {
                return ( IModelDatabaseFeatures ) ActivatorUtilities.CreateInstance( serviceProvider, Builder.ModelDatabaseFeaturesType );
            } );
        }

        /// <summary>
        /// Gives the extension a chance to validate that all options in the extension are valid.
        /// Most extensions do not have invalid combinations and so this will be a no-op.
        /// If options are invalid, then an exception should be thrown.
        /// </summary>
        /// <param name="options"> The options being validated. </param>
        public void Validate( IDbContextOptions options )
        {
            if ( Builder.ModelDatabaseFeaturesType == null )
            {
                throw new Exception( "Database provider has not been fully configured." );
            }
        }

        #endregion

        #region Support Classes

        /// <summary>
        /// Defines the extension information for <see cref="ModelDbContextOptionsExtension"/> class.
        /// </summary>
        private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
        {
            public ExtensionInfo( IDbContextOptionsExtension extension )
                : base( extension )
            {
            }

            public new ModelDbContextOptionsExtension Extension
                => ( ModelDbContextOptionsExtension ) base.Extension;

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
