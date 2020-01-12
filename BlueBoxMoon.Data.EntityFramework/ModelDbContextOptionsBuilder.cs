using System;
using System.Collections.Generic;

using BlueBoxMoon.Data.EntityFramework.Internals;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Provides a fluent interface to building the options for the
    /// <see cref="ModelDbContext"/> class.
    /// </summary>
    public class ModelDbContextOptionsBuilder
    {
        #region Properties

        /// <summary>
        /// Retrieves the built options from this builder.
        /// </summary>
        public ModelDbContextOptions Options
        {
            get
            {
                if ( ModelDatabaseFeaturesType == null )
                {
                    throw new Exception( "Database provider has not been configured." );
                }

                return _options;
            }
        }

        /// <summary>
        /// The type that will provide services unique to each database
        /// provider.
        /// </summary>
        internal Type ModelDatabaseFeaturesType { get; set; }

        #endregion

        #region Fields

        private readonly ModelDbContextOptions _options = new ModelDbContextOptions();

        #endregion

        #region Methods

        /// <summary>
        /// Sets the database provider features type that will provide
        /// database-specific functionality.
        /// </summary>
        /// <typeparam name="T">A type that implements <see cref="IModelDatabaseFeatures"/>.</typeparam>
        /// <returns>An <see cref="ModelDbContextOptionsBuilder"/> that can be used to further configure options.</returns>
        internal ModelDbContextOptionsBuilder UseDatabaseProviderFeatures<T>()
            where T : IModelDatabaseFeatures
        {
            ModelDatabaseFeaturesType = typeof( T );

            return this;
        }

        /// <summary>
        /// Configures the ModelDbContextOptions to use a pre-save hook delegate.
        /// </summary>
        /// <param name="hook">The delegate to be called before saving.</param>
        /// <returns>An <see cref="ModelDbContextOptionsBuilder"/> that can be used to further configure options.</returns>
        public virtual ModelDbContextOptionsBuilder UsePreSaveHook( ModelDbContextSaveHooks.PreSaveHook hook )
        {
            var saveHook = new ModelDbContextSaveHooks( hook, null, null );

            ( ( List<ModelDbContextSaveHooks> ) Options.SaveHooks ).Add( saveHook );

            return this;
        }

        /// <summary>
        /// Configures the ModelDbContextOptions to use a post-save hook delegate.
        /// The delegate's <para>success</para> parameter will indicate if the
        /// save succeeded.
        /// </summary>
        /// <param name="hook">The delegate to be called after saving.</param>
        /// <returns>An <see cref="ModelDbContextOptionsBuilder"/> that can be used to further configure options.</returns>
        public virtual ModelDbContextOptionsBuilder UsePostSaveHook( ModelDbContextSaveHooks.PostSaveHook hook )
        {
            var saveHook = new ModelDbContextSaveHooks( null, hook, null );

            ( ( List<ModelDbContextSaveHooks> ) Options.SaveHooks ).Add( saveHook );

            return this;
        }

        /// <summary>
        /// Configures the ModelDbContextOptions to use a class that implements
        /// <see cref="IModelSaveHook"/> to process both pre-save and post-save
        /// hook calls. 
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IModelSaveHook"/>.</typeparam>
        /// <returns>An <see cref="ModelDbContextOptionsBuilder"/> that can be used to further configure options.</returns>
        public virtual ModelDbContextOptionsBuilder UseSaveHook<T>()
            where T : IModelSaveHook
        {
            var saveHook = new ModelDbContextSaveHooks( null, null, typeof( T ) );

            ( ( List<ModelDbContextSaveHooks> ) Options.SaveHooks ).Add( saveHook );

            return this;
        }

        #endregion
    }
}
