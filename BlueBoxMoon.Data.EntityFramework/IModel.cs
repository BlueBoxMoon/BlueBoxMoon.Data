using System;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Defines the basic requirements of a model in the database.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// The unique identifier of the model.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// The globally unique identifier of the model.
        /// </summary>
        Guid Guid { get; set; }
    }
}
