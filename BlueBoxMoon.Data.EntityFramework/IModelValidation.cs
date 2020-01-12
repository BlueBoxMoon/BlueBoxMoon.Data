using FluentValidation;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Defines the basic requirements of a model that supports pre-save
    /// validation.
    /// </summary>
    public interface IModelValidation
    {
        /// <summary>
        /// Gets the validator that will validate this instance.
        /// </summary>
        /// <returns>
        /// An <see cref="IValidator"/> instance or null if no validation
        /// needs to be performed.
        /// </returns>
        /// <remarks>
        /// Validators can and should be singleton instances for
        /// performance reasons.
        /// </remarks>
        IValidator GetValidator();
    }
}
