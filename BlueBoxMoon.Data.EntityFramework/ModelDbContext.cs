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

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace BlueBoxMoon.Data.EntityFramework
{
    public class ModelDbContext : DbContext
    {
        #region Properties

        /// <summary>
        /// If <c>true</c> then pre/post processing calls are disabled during SaveChanges().
        /// </summary>
        public bool DisablePrePostProcessing { get; set; }

        /// <summary>
        /// Defines the options used by this database context.
        /// </summary>
        protected ModelDbContextOptions ModelOptions { get; }

        #endregion

        #region Fields

        /// <summary>
        /// Identifies any save hooks that successfully ran during pre-save
        /// that should be run during post-save.
        /// </summary>
        private List<IModelSaveHook> _pendingPostSaveContextHooks;

        /// <summary>
        /// Identifies any <see cref="IModelChanges"/> instances that need
        /// their post-save hooks called.
        /// </summary>
        private List<IModelChanges> _pendingPostSaveModelHooks;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelDbContext"/> class.
        /// </summary>
        /// <param name="options">The context options used to initialize this database context.</param>
        public ModelDbContext( DbContextOptions options )
            : base( options )
        {
            var extension = options.FindExtension<ModelDbContextOptionsExtension>();

            if ( extension != null )
            {
                ModelOptions = extension.Builder.Options;
            }
            else
            {
                ModelOptions = new ModelDbContextOptions();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Persists all changes into the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">If <c>true</c> then the change tracker will accept all changes on success.</param>
        /// <returns>The number of records that were modified.</returns>
        public sealed override int SaveChanges( bool acceptAllChangesOnSuccess )
        {
            ValidateModels();

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
            ValidateModels();

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
        /// Validates all modified <see cref="IModel"/> instances to ensure
        /// they are valid before attempting to save to the database.
        /// </summary>
        protected virtual void ValidateModels()
        {
            var exceptions = new List<Exception>();

            var models = ChangeTracker.Entries()
                .Where( a => a.State == EntityState.Added || a.State == EntityState.Modified )
                .Where( a => a.Entity is IModelValidation )
                .Select( a => a.Entity )
                .Cast<IModelValidation>()
                .ToList();

            foreach ( var model in models )
            {
                var validator = model.GetValidator();

                if ( validator == null )
                {
                    continue;
                }

                var result = validator.Validate( model );

                if ( !result.IsValid )
                {
                    exceptions.Add( new ValidationException( result.Errors ) );
                }
            }

            if ( exceptions.Any() )
            {
                throw new AggregateException( "One or more models failed validation.", exceptions );
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
            _pendingPostSaveContextHooks = new List<IModelSaveHook>();
            _pendingPostSaveModelHooks = new List<IModelChanges>();

            CallPreSaveModelHooks();

            CallPreSaveContextHooks();
        }

        /// <summary>
        /// Initiates any calls to any <see cref="IModelChanges"/> instances
        /// to give them a chance to do final preparation for saving.
        /// </summary>
        private void CallPreSaveModelHooks()
        {
            var entries = ChangeTracker.Entries()
                .Where( a => a.State == EntityState.Added || a.State == EntityState.Modified )
                .Where( a => a.Entity is IModelChanges )
                .ToList();

            if ( entries.Count > 0 )
            {
                foreach ( var entry in entries )
                {
                    var model = ( IModelChanges ) entry.Entity;

                    try
                    {
                        model.PreSaveChanges( this, entry );
                    }
                    catch ( Exception ex )
                    {
                        CallPendingPostSaveContextHooks( false );

                        CallPendingPostSaveModelHooks( false );

                        throw ex;
                    }

                    _pendingPostSaveModelHooks.Add( model );
                }
            }
        }

        /// <summary>
        /// Initiates any calls to pre-save hooks defined on the context.
        /// </summary>
        private void CallPreSaveContextHooks()
        {
            _pendingPostSaveContextHooks = new List<IModelSaveHook>();

            //
            // Call pre-save hooks.
            //
            foreach ( var saveHook in ModelOptions.SaveHooks )
            {
                try
                {
                    if ( saveHook.HookType != null )
                    {
                        var hook = ( IModelSaveHook ) Activator.CreateInstance( saveHook.HookType );

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

                    CallPendingPostSaveModelHooks( false);

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
            for ( int i = 0; i < ModelOptions.SaveHooks.Count; i++ )
            {
                try
                {
                    ModelOptions.SaveHooks[i].PostSave?.Invoke( this, success );
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
            exceptions.AddRange( CallPendingPostSaveModelHooks( success ) );

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
        /// Calls any model hooks that are pending and need to be called.
        /// </summary>
        /// <param name="success"><c>true</c> if the save was successful.</param>
        private List<Exception> CallPendingPostSaveModelHooks( bool success )
        {
            var exceptions = new List<Exception>();

            if ( _pendingPostSaveModelHooks == null )
            {
                return exceptions;
            }

            //
            // Call any previous hooks that need to be told the save
            // failed.
            //
            foreach ( var pendingHook in _pendingPostSaveModelHooks )
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

            _pendingPostSaveModelHooks = null;

            return exceptions;
        }

        #endregion
    }
}
