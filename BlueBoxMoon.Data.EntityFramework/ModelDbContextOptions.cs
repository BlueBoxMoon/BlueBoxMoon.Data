using System.Collections.Generic;

using BlueBoxMoon.Data.EntityFramework.Internals;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Defines the configuration options that will be used by
    /// <see cref="ModelDbContext"/> instances.
    /// </summary>
    public class ModelDbContextOptions
    {
        #region Properties

        /// <summary>
        /// The pre- and post-save hooks that will be used. The order these
        /// hooks are called is not guaranteed.
        /// </summary>
        public IReadOnlyList<ModelDbContextSaveHooks> SaveHooks { get; } = new List<ModelDbContextSaveHooks>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelDbContextOptions"/> class.
        /// </summary>
        internal ModelDbContextOptions()
        {
        }

        #endregion
    }
}
