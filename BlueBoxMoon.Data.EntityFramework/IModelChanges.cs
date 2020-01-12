using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Defines methods that will be called before and after a model is saved.
    /// </summary>
    public interface IModelChanges
    {
        /// <summary>
        /// Called before this model is saved to the database.
        /// </summary>
        /// <param name="dbContext">The database context owning this instance.</param>
        /// <param name="entry">The Entity Framework entry about this instance.</param>
        void PreSaveChanges( ModelDbContext dbContext, EntityEntry entry );

        /// <summary>
        /// Called after this model has been saved to the database. If the
        /// PreSaveChanges() was called, then it is guaranteed that this method
        /// will also be called.
        /// </summary>
        /// <param name="dbContext">The database context owning this instance.</param>
        /// <param name="success"><c>true</c> if the save was successful.</param>
        void PostSaveChanges( ModelDbContext dbContext, bool success );
    }
}
