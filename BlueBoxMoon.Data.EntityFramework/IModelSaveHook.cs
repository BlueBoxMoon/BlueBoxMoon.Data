namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Defines an interface that can be implemented by classes wishing to
    /// be used as pre- and post-save hooks for ModelDbContext instances.
    /// </summary>
    public interface IModelSaveHook
    {
        /// <summary>
        /// Called just before changes are saved to the database.
        /// </summary>
        /// <param name="modelDbContext">The database context.</param>
        void PreSaveChanges( ModelDbContext modelDbContext );

        /// <summary>
        /// Called just after changes are saved to the database.
        /// </summary>
        /// <param name="modelDbContext">The database context.</param>
        /// <param name="success"><c>true</c> if the save operation succeeded.</param>
        void PostSaveChanges( ModelDbContext modelDbContext, bool success );
    }
}
