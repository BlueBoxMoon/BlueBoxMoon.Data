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
using System.ComponentModel.DataAnnotations;
using FluentValidation;

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Implementation of most common interfaces used by this library.
    /// </summary>
    /// <seealso cref="BlueBoxMoon.Data.EntityFramework.IEntity" />
    /// <seealso cref="BlueBoxMoon.Data.EntityFramework.IEntityChanges" />
    /// <seealso cref="BlueBoxMoon.Data.EntityFramework.IEntityValidation" />
    public abstract class Entity : IEntity, IEntityChanges, IEntityValidation
    {
        private static readonly IValidator _validator = new EntityValidator();

        #region Properties

        /// <summary>
        /// The unique identifier of the entity.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// The globally unique identifier of the entity.
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        #endregion

        #region Methods

        public virtual void PreSaveChanges( EntityDbContext dbContext, EntityEntry entry )
        {
        }

        public virtual void PostSaveChanges( EntityDbContext dbContext, bool success )
        {
        }

        public virtual IValidator GetValidator()
        {
            return _validator;
        }

        #endregion

        #region Validator

        private class EntityValidator : AbstractValidator<Entity>
        {
            public EntityValidator()
            {
                RuleFor( a => a.Guid ).NotEmpty();
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines an interface that allows for objects to store custom extension data.
    /// </summary>
    public interface IExtensible
    {
        /// <summary>
        /// Finds the extension for the given type associated with this instance.
        /// </summary>
        /// <typeparam name="T">The type of extension to retrieve.</typeparam>
        /// <returns>An instance of <typeparamref name="T"/> or <c>null</c> if not found.</returns>
        T FindExtension<T>();

        /// <summary>
        /// Gets the extension for the given type associated with this instance. Throws
        /// an exception if extension is not found.
        /// </summary>
        /// <typeparam name="T">The type of extension to retrieve.</typeparam>
        /// <returns>An instance of <typeparamref name="T"/>.</returns>
        T GetExtension<T>();

        /// <summary>
        /// Adds or updates (replaces) the extension.
        /// </summary>
        /// <typeparam name="T">The type of extension to be stored.</typeparam>
        /// <param name="extension">The extension instance.</param>
        void AddOrUpdateExtension<T>( T extension );
    }
}
