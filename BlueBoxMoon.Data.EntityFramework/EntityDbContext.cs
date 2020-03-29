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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BlueBoxMoon.Data.EntityFramework.Internals;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Provides all the automation and resources required for a DbContext
    /// to work with this library.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class EntityDbContext : DbContext
    {
        #region Properties

        /// <summary>
        /// If <c>true</c> then pre/post processing calls are disabled during SaveChanges().
        /// </summary>
        public bool DisablePrePostProcessing { get; set; }

        /// <summary>
        /// Defines the options used by this database context.
        /// </summary>
        public EntityDbContextOptions EntityContextOptions { get; }

        /// <summary>
        /// Contains the options this context was constructed with.
        /// </summary>
        protected DbContextOptions ContextOptions { get; }
        
        #endregion

        #region Fields

        /// <summary>
        /// Identifies any save hooks that successfully ran during pre-save
        /// that should be run during post-save.
        /// </summary>
        private List<IEntitySaveHook> _pendingPostSaveContextHooks;

        /// <summary>
        /// Identifies any <see cref="IEntityChanges"/> instances that need
        /// their post-save hooks called.
        /// </summary>
        private List<IEntityChanges> _pendingPostSaveEntityHooks;

        /// <summary>
        /// Contains the <see cref="DataSet{T}"/> objects already instantiated
        /// with this context.
        /// </summary>
        private readonly Dictionary<Type, object> _dataSets = new Dictionary<Type, object>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDbContext"/> class.
        /// </summary>
        /// <param name="options">The context options used to initialize this database context.</param>
        public EntityDbContext( DbContextOptions options )
            : base( options )
        {
            ContextOptions = options;

            var extension = options.FindExtension<EntityDbContextOptionsExtension>();

            if ( extension != null )
            {
                EntityContextOptions = extension.Builder.Options;
            }
            else
            {
                EntityContextOptions = new EntityDbContextOptions();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">
        /// The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.
        /// </param>
        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );

            var entityTypes = EntityContextOptions.RegisteredEntities
                .Keys
                .ToList();

            //
            // Configure all registered entities.
            //
            foreach ( var type in entityTypes )
            {
                var entityBuilder = modelBuilder.Entity( type );

                //
                // If it's an IEntity, ensure the Guid property is marked as unique.
                //
                if ( typeof( IEntity ).IsAssignableFrom( type ) )
                {
                    entityBuilder.HasIndex( nameof( Entity.Guid ) )
                        .IsUnique();
                }

                //
                // If it's an Entity, mark it for changed notifications tracking.
                //
                if ( typeof( Entity ).IsAssignableFrom( type ) )
                {
                    entityBuilder.HasChangeTrackingStrategy( ChangeTrackingStrategy.ChangedNotifications );
                }
            }

            //
            // Apply any configurations from assemblies containing
            // registered types.
            //
            foreach ( var assembly in entityTypes.Select( a => a.Assembly ).Distinct() )
            {
                modelBuilder.ApplyConfigurationsFromAssembly( assembly );
            }

            //
            // Call plugin OnModelCreating(this, modelBuilder) methods.
            //
        }

        /// <summary>
        /// Persists all changes into the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">If <c>true</c> then the change tracker will accept all changes on success.</param>
        /// <returns>The number of records that were modified.</returns>
        public sealed override int SaveChanges( bool acceptAllChangesOnSuccess )
        {
            ValidateEntities();

            if ( DisablePrePostProcessing )
            {
                return base.SaveChanges( acceptAllChangesOnSuccess );
            }

            int changes;

            PreSaveChanges();

            try
            {
                changes = base.SaveChanges( acceptAllChangesOnSuccess );
            }
            catch ( Exception ex )
            {
                try
                {
                    PostSaveChanges( false );
                }
                finally
                {
                    throw ex;
                }
            }

            PostSaveChanges( true );

            return changes;
        }

        /// <summary>
        /// Persists all changes into the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">If <c>true</c> then the change tracker will accept all changes on success.</param>
        /// <param name="cancellationToken">A cancellation token to be used if the operation should be cancelled.</param>
        /// <returns>The number of records that were modified.</returns>
        public sealed override async Task<int> SaveChangesAsync( bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default )
        {
            ValidateEntities();

            if ( DisablePrePostProcessing )
            {
                return await base.SaveChangesAsync( acceptAllChangesOnSuccess, cancellationToken );
            }

            int changes;

            PreSaveChanges();

            try
            {
                changes = await base.SaveChangesAsync( acceptAllChangesOnSuccess, cancellationToken );
            }
            catch ( Exception ex )
            {
                try
                {
                    PostSaveChanges( false );
                }
                finally
                {
                    throw ex;
                }
            }

            PostSaveChanges( true );

            return changes;
        }

        /// <summary>
        /// Gets the data set for <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>A <see cref="DataSet{TEntity}"/> that can be used to interact with the database.</returns>
        public virtual DataSet<TEntity> GetDataSet<TEntity>()
            where TEntity : class, IEntity
        {
            var entityType = typeof( TEntity );

            if ( !_dataSets.ContainsKey( entityType ) )
            {
                var setType = EntityContextOptions.RegisteredEntities[entityType].DataSetType;

                _dataSets.Add( entityType, ActivatorUtilities.CreateInstance( this.GetInfrastructure(), setType, this ) );
            }

            return ( DataSet<TEntity> ) _dataSets[entityType];
        }

        /// <summary>
        /// Gets the data set for <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>A <see cref="DataSet{TEntity}"/> that can be used to interact with the database.</returns>
        public virtual TSet GetDataSet<TEntity, TSet>()
            where TEntity : class, IEntity
            where TSet : DataSet<TEntity>
        {
            var entityType = typeof( TEntity );

            if ( !_dataSets.ContainsKey( entityType ) )
            {
                var setType = typeof( TSet );

                _dataSets.Add( entityType, ActivatorUtilities.CreateInstance( this.GetInfrastructure(), setType, this ) );
            }

            return ( TSet ) _dataSets[entityType];
        }

        /// <summary>
        /// Validates all modified <see cref="IEntityValidation"/> instances to ensure
        /// they are valid before attempting to save to the database.
        /// </summary>
        protected virtual void ValidateEntities()
        {
            var exceptions = new List<Exception>();

            var entities = ChangeTracker.Entries()
                .Where( a => a.State == EntityState.Added || a.State == EntityState.Modified )
                .Where( a => a.Entity is IEntityValidation )
                .Select( a => a.Entity )
                .Cast<IEntityValidation>()
                .ToList();

            foreach ( var entity in entities )
            {
                var result = entity.Validate();

                if ( result == null || result.IsValid )
                {
                    continue;
                }

                string message;

                if ( entity is Entity )
                {
                    message = $"The {entity.GetType().Name} entity {( ( Entity ) entity ).Guid} failed validation.";
                }
                else
                {
                    message = $"The {entity.GetType().Name} entity failed validation.";
                }

                var exception = new AggregateException( message, result.Errors.Select( a => new ValidationException( a ) ) );
                exceptions.Add( exception );
            }

            if ( exceptions.Any() )
            {
                throw new AggregateException( "One or more entities failed validation.", exceptions );
            }
        }

        /// <summary>
        /// Called immediately before a save operation begins.
        /// </summary>
        /// <remarks>
        /// When overridden in a subclass, it is suggested that the base method
        /// be called first.
        /// </remarks>
        protected virtual void PreSaveChanges()
        {
            _pendingPostSaveContextHooks = new List<IEntitySaveHook>();
            _pendingPostSaveEntityHooks = new List<IEntityChanges>();

            CallPreSaveEntityHooks();

            CallPreSaveContextHooks();
        }

        /// <summary>
        /// Initiates any calls to any <see cref="IEntityChanges"/> instances
        /// to give them a chance to do final preparation for saving.
        /// </summary>
        private void CallPreSaveEntityHooks()
        {
            var entries = ChangeTracker.Entries()
                .Where( a => a.State == EntityState.Added || a.State == EntityState.Modified )
                .Where( a => a.Entity is IEntityChanges )
                .ToList();

            if ( entries.Count > 0 )
            {
                foreach ( var entry in entries )
                {
                    var entity = ( IEntityChanges ) entry.Entity;

                    try
                    {
                        entity.PreSaveChanges( this, entry );
                    }
                    catch ( Exception ex )
                    {
                        CallPendingPostSaveContextHooks( false );

                        CallPendingPostSaveEntityHooks( false );

                        throw ex;
                    }

                    _pendingPostSaveEntityHooks.Add( entity );
                }
            }
        }

        /// <summary>
        /// Initiates any calls to pre-save hooks defined on the context.
        /// </summary>
        private void CallPreSaveContextHooks()
        {
            _pendingPostSaveContextHooks = new List<IEntitySaveHook>();

            //
            // Call pre-save hooks.
            //
            foreach ( var saveHook in EntityContextOptions.SaveHooks )
            {
                try
                {
                    if ( saveHook.HookType != null )
                    {
                        var hook = ( IEntitySaveHook ) Activator.CreateInstance( saveHook.HookType );

                        hook.PreSaveChanges( this );

                        _pendingPostSaveContextHooks.Add( hook );
                    }
                    else
                    {
                        saveHook.PreSave?.Invoke( this );
                    }
                }
                catch ( Exception ex )
                {
                    CallPendingPostSaveContextHooks( false );

                    CallPendingPostSaveEntityHooks( false);

                    throw ex;
                }
            }
        }

        /// <summary>
        /// Called immediately after a save operation has completed.
        /// </summary>
        /// <param name="success"><c>true</c> if the save was successful.</param>
        /// <remarks>
        /// When overridden in a subclass, it is suggested that the base method
        /// be called first.
        /// </remarks>
        protected virtual void PostSaveChanges( bool success )
        {
            var exceptions = new List<Exception>();

            //
            // Call post-save hooks.
            //
            for ( int i = 0; i < EntityContextOptions.SaveHooks.Count; i++ )
            {
                try
                {
                    EntityContextOptions.SaveHooks[i].PostSave?.Invoke( this, success );
                }
                catch ( Exception ex )
                {
                    exceptions.Add( ex );
                }
            }

            //
            // Call any pending post-save hooks.
            //
            exceptions.AddRange( CallPendingPostSaveContextHooks( success ) );
            exceptions.AddRange( CallPendingPostSaveEntityHooks( success ) );

            if ( exceptions.Count > 0 )
            {
                throw new AggregateException( "Error running post-save hooks.", exceptions );
            }

        }

        /// <summary>
        /// Calls any previous context hooks that need to be informed about
        /// the save status.
        /// </summary>
        /// <param name="success"><c>true</c> if the save was successful.</param>
        private List<Exception> CallPendingPostSaveContextHooks( bool success )
        {
            var exceptions = new List<Exception>();

            if ( _pendingPostSaveContextHooks == null )
            {
                return exceptions;
            }

            //
            // Call any previous hooks that need to be told the save
            // failed.
            //
            foreach ( var contextHook in _pendingPostSaveContextHooks )
            {
                try
                {
                    contextHook.PostSaveChanges( this, success );
                }
                catch ( Exception ex )
                {
                    exceptions.Add( ex );
                }
            }

            _pendingPostSaveContextHooks = null;

            return exceptions;
        }

        /// <summary>
        /// Calls any <see cref="Entity"/> hooks that are pending and need to be called.
        /// </summary>
        /// <param name="success"><c>true</c> if the save was successful.</param>
        private List<Exception> CallPendingPostSaveEntityHooks( bool success )
        {
            var exceptions = new List<Exception>();

            if ( _pendingPostSaveEntityHooks == null )
            {
                return exceptions;
            }

            //
            // Call any previous hooks that need to be told the save
            // failed.
            //
            foreach ( var pendingHook in _pendingPostSaveEntityHooks )
            {
                try
                {
                    pendingHook.PostSaveChanges( this, success );
                }
                catch ( Exception ex )
                {
                    exceptions.Add( ex );
                }
            }

            _pendingPostSaveEntityHooks = null;

            return exceptions;
        }

        #endregion
    }
}
