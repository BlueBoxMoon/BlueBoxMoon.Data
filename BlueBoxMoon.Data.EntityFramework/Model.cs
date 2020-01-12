using System;

using FluentValidation;

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BlueBoxMoon.Data.EntityFramework
{
    public abstract class Model : IModel, IModelChanges, IModelValidation
    {
        private static readonly IValidator _validator = new ModelValidator();

        #region Properties

        /// <summary>
        /// The unique identifier of the model.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The globally unique identifier of the model.
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        #endregion

        #region Methods

        public virtual void PreSaveChanges( ModelDbContext dbContext, EntityEntry entry )
        {
        }

        public virtual void PostSaveChanges( ModelDbContext dbContext, bool success )
        {
        }

        public virtual IValidator GetValidator()
        {
            return _validator;
        }

        #endregion

        #region Validator

        private class ModelValidator : AbstractValidator<Model>
        {
            public ModelValidator()
            {
                RuleFor( a => a.Guid ).NotEmpty();
            }
        }

        #endregion
    }
}
