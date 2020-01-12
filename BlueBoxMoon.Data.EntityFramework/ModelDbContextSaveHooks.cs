using System;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Used internally to track pre- and post-save hooks.
    /// </summary>
    public class ModelDbContextSaveHooks
    {
        #region Delegates

        /// <summary>
        /// A pre-save hook that will be called before the changes are saved.
        /// </summary>
        /// <param name="modelDbContext">The database context.</param>
        public delegate void PreSaveHook( ModelDbContext modelDbContext );

        /// <summary>
        /// A post-save hook that will be called before the changes are saved.
        /// </summary>
        /// <param name="modelDbContext">The database context.</param>
        /// <param name="success"><c>true</c> if the save was successful.</param>
        public delegate void PostSaveHook( ModelDbContext modelDbContext, bool success );

        #endregion

        #region Properties

        /// <summary>
        /// Called just before the save occurs.
        /// </summary>
        public PreSaveHook PreSave { get; }

        /// <summary>
        /// Called just after the save occurrs.
        /// </summary>
        public PostSaveHook PostSave { get; }

        /// <summary>
        /// A type that will be instantiated to provide the PreSave and
        /// PostSave methods. Must implement the IModelSaveHook interface.
        /// </summary>
        public Type HookType { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelDbContextSaveHooks"/> class.
        /// </summary>
        /// <param name="preSave">The pre-save hook to call.</param>
        /// <param name="postSave">The post-save hook to call.</param>
        /// <param name="type">The type which implements IModelSaveHook.</param>
        internal ModelDbContextSaveHooks( PreSaveHook preSave, PostSaveHook postSave, Type type )
        {
            if ( type != null && !typeof( IModelSaveHook ).IsAssignableFrom( type ) )
            {
                throw new ArgumentException( "Type does not implement IModelSaveHook.", nameof( type ) );
            }

            if ( ( preSave != null ? 1 : 0 ) + ( postSave != null ? 1 : 0 ) + ( type != null ? 1 : 0 ) > 1 )
            {
                throw new ArgumentException( "Can only specify one of preSave, postSave and type." );
            }

            PreSave = preSave;
            PostSave = postSave;
            HookType = type;
        }

        #endregion
    }
}
