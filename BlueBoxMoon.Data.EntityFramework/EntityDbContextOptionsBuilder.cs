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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Provides a fluent interface to building the options for the
    /// <see cref="EntityDbContext"/> class.
    /// </summary>
    public class EntityDbContextOptionsBuilder
    {
        #region Properties

        /// <summary>
        /// Retrieves the built options from this builder.
        /// </summary>
        public EntityDbContextOptions Options
        {
            get
            {
                if ( EntityDatabaseFeaturesType == null )
                {
                    throw new Exception( "Database provider has not been configured." );
                }

                return _options;
            }
        }

        /// <summary>
        /// The base options builder.
        /// </summary>
        public DbContextOptionsBuilder BaseOptionsBuilder { get; }

        /// <summary>
        /// The type that will provide services unique to each database
        /// provider.
        /// </summary>
        internal Type EntityDatabaseFeaturesType { get; set; }

        /// <summary>
        /// Gets the apply service actions.
        /// </summary>
        /// <value>
        /// The apply service actions.
        /// </value>
        internal List<Action<IServiceCollection>> ApplyServiceActions { get; } = new List<Action<IServiceCollection>>();

        #endregion

        #region Fields

        private readonly EntityDbContextOptions _options = new EntityDbContextOptions();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="EntityDbContextOptionsBuilder"/> class.
        /// </summary>
        /// <param name="baseOptionsBuilder">The base options builder.</param>
        public EntityDbContextOptionsBuilder( DbContextOptionsBuilder baseOptionsBuilder )
        {
            BaseOptionsBuilder = baseOptionsBuilder;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers the entity to be used in the system.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TSet">The type of the data set.</typeparam>
        /// <returns>An <see cref="EntityDbContextOptionsBuilder"/> that can be used to further configure options.</returns>
        public EntityDbContextOptionsBuilder RegisterEntity<TEntity, TSet>()
        {
            var entityOptions = new EntitySettings
            {
                DataSetType = typeof( TSet )
            };

            ( ( Dictionary<Type, EntitySettings> ) Options.RegisteredEntities ).Add( typeof( TEntity ), entityOptions );

            return this;
        }

        /// <summary>
        /// Sets the database provider features type that will provide
        /// database-specific functionality.
        /// </summary>
        /// <typeparam name="T">A type that implements <see cref="IEntityDatabaseFeatures"/>.</typeparam>
        /// <returns>An <see cref="EntityDbContextOptionsBuilder"/> that can be used to further configure options.</returns>
        public EntityDbContextOptionsBuilder UseDatabaseProviderFeatures<T>()
            where T : IEntityDatabaseFeatures
        {
            EntityDatabaseFeaturesType = typeof( T );

            return this;
        }

        /// <summary>
        /// Calls the action method when the services can be applied to Entity Framework.
        /// </summary>
        /// <param name="applyServicesAction">The apply services action.</param>
        /// <returns>An <see cref="EntityDbContextOptionsBuilder"/> that can be used to further configure options.</returns>
        public EntityDbContextOptionsBuilder ApplyServices( Action<IServiceCollection> applyServicesAction )
        {
            ApplyServiceActions.Add( applyServicesAction );

            return this;
        }

        /// <summary>
        /// Configures the <see cref="EntityDbContextOptions"/> to use a pre-save hook delegate.
        /// </summary>
        /// <param name="hook">The delegate to be called before saving.</param>
        /// <returns>An <see cref="EntityDbContextOptionsBuilder"/> that can be used to further configure options.</returns>
        public virtual EntityDbContextOptionsBuilder UsePreSaveHook( EntityDbContextSaveHooks.PreSaveHook hook )
        {
            var saveHook = new EntityDbContextSaveHooks( hook, null, null );

            ( ( List<EntityDbContextSaveHooks> ) Options.SaveHooks ).Add( saveHook );

            return this;
        }

        /// <summary>
        /// Configures the <see cref="EntityDbContextOptions"/> to use a post-save hook delegate.
        /// The delegate's <para>success</para> parameter will indicate if the
        /// save succeeded.
        /// </summary>
        /// <param name="hook">The delegate to be called after saving.</param>
        /// <returns>An <see cref="EntityDbContextOptionsBuilder"/> that can be used to further configure options.</returns>
        public virtual EntityDbContextOptionsBuilder UsePostSaveHook( EntityDbContextSaveHooks.PostSaveHook hook )
        {
            var saveHook = new EntityDbContextSaveHooks( null, hook, null );

            ( ( List<EntityDbContextSaveHooks> ) Options.SaveHooks ).Add( saveHook );

            return this;
        }

        /// <summary>
        /// Configures the <see cref="EntityDbContextOptions"/> to use a class that implements
        /// <see cref="IEntitySaveHook"/> to process both pre-save and post-save
        /// hook calls. 
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IEntitySaveHook"/>.</typeparam>
        /// <returns>An <see cref="EntityDbContextOptionsBuilder"/> that can be used to further configure options.</returns>
        public virtual EntityDbContextOptionsBuilder UseSaveHook<T>()
            where T : IEntitySaveHook
        {
            var saveHook = new EntityDbContextSaveHooks( null, null, typeof( T ) );

            ( ( List<EntityDbContextSaveHooks> ) Options.SaveHooks ).Add( saveHook );

            return this;
        }

        #endregion
    }
}
