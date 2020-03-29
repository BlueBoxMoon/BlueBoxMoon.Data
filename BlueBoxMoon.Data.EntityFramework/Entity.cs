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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

using FluentValidation;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Implementation of most common interfaces used by this library.
    /// </summary>
    /// <seealso cref="BlueBoxMoon.Data.EntityFramework.IEntity" />
    /// <seealso cref="BlueBoxMoon.Data.EntityFramework.IEntityChanges" />
    /// <seealso cref="BlueBoxMoon.Data.EntityFramework.IEntityValidation" />
    public abstract class Entity : IEntity, IEntityChanges, IEntityValidation, INotifyPropertyChanged, IExtensible
    {
        #region Fields

        /// <summary>
        /// The validator to use for this type.
        /// </summary>
        private static readonly IValidator _validator = new EntityValidator();

        /// <summary>
        /// Backing store that holds property values.
        /// </summary>
        private Dictionary<string, object> _properties = new Dictionary<string, object>();

        /// <summary>
        /// Backing store that holds all extension objects.
        /// </summary>
        private Dictionary<Type, object> _extensions = new Dictionary<Type, object>();

        /// <summary>
        /// Backing store that holds the <see cref="EntityDbContext"/> for this entity.
        /// </summary>
        private EntityDbContext _dbContext;

        #endregion

        #region Events

        /// <summary>
        /// Notifies listeners that a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Entity Properties

        /// <summary>
        /// The unique identifier of the entity.
        /// </summary>
        [Key]
        [Required]
        public long Id
        {
            get => ( long? ) GetValue() ?? 0;
            set => SetValue( value );
        }

        /// <summary>
        /// The globally unique identifier of the entity.
        /// </summary>
        [Required]
        public Guid Guid
        {
            get => ( Guid ) GetValue();
            set => SetValue( value );
        }

        /// <summary>
        /// The timestamp that this entity was initially created.
        /// </summary>
        [Required]
        public DateTime CreatedDateTime
        {
            get => ( DateTime ) GetValue();
            set => SetValue( value );
        }

        /// <summary>
        /// The timestamp that this entity was last modified.
        /// </summary>
        [Required]
        public DateTime ModifiedDateTime
        {
            get => ( DateTime ) GetValue();
            set => SetValue( value );
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the database context this entity is associated with.
        /// </summary>
        [IgnoreDataMember]
        [NotMapped]
        public EntityDbContext DbContext
        {
            get
            {
                if ( _dbContext == null )
                {
                    var lazyLoader = ( ILazyLoader ) GetType().GetProperties()
                        .FirstOrDefault( a => a.PropertyType == typeof( ILazyLoader ) )
                        ?.GetValue( this );
                    
                    if ( lazyLoader != null )
                    {
                        var getDbContext = lazyLoader.GetType()
                            .GetProperty( "Context", BindingFlags.Instance | BindingFlags.NonPublic );

                        _dbContext = getDbContext?.GetValue( lazyLoader ) as EntityDbContext;
                    }
                }

                return _dbContext;
            }
            set => _dbContext = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity()
        {
            Guid = Guid.NewGuid();
            CreatedDateTime = ModifiedDateTime = DateTime.Now;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called before this entity is saved to the database.
        /// </summary>
        /// <param name="dbContext">The database context owning this instance.</param>
        /// <param name="entry">The Entity Framework entry about this instance.</param>
        public virtual void PreSaveChanges( EntityDbContext dbContext, EntityEntry entry )
        {
            ModifiedDateTime = DateTime.Now;
        }

        /// <summary>
        /// Called after this entity has been saved to the database. If the
        /// PreSaveChanges() was called, then it is guaranteed that this method
        /// will also be called.
        /// </summary>
        /// <param name="dbContext">The database context owning this instance.</param>
        /// <param name="success"><c>true</c> if the save was successful.</param>
        public virtual void PostSaveChanges( EntityDbContext dbContext, bool success )
        {
        }

        /// <summary>
        /// Gets the validator that will validate this instance.
        /// </summary>
        /// <returns>
        /// An <see cref="IValidator"/> instance or null if no validation
        /// needs to be performed.
        /// </returns>
        public virtual IValidator GetValidator()
        {
            return _validator;
        }

        /// <summary>
        /// Gets the value of a property from the backing store.
        /// </summary>
        /// <param name="propertyName">The name of the property to get.</param>
        /// <returns>The value of the property or <c>null</c> if not found.</returns>
        protected object GetValue( [CallerMemberName] string propertyName = null )
        {
            if ( _properties.TryGetValue( propertyName, out var value ) )
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the value of a property in the backing store.
        /// </summary>
        /// <param name="value">The value to be set.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        protected virtual void SetValue( object value, [CallerMemberName] string propertyName = null )
        {
            if ( !Equals( value, GetValue( propertyName ) ) )
            {
                _properties[propertyName] = value;
                OnPropertyChanged( propertyName );
            }
        }

        /// <summary>
        /// Called when a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        #endregion

        #region IExtensible

        /// <summary>
        /// Finds the extension for the given type associated with this instance.
        /// </summary>
        /// <typeparam name="TExtension">The type of extension to retrieve.</typeparam>
        /// <returns>An instance of <typeparamref name="TExtension"/> or <c>null</c> if not found.</returns>
        public TExtension FindExtension<TExtension>()
            where TExtension : class
        {
            if ( _extensions.TryGetValue( typeof( TExtension ), out var extension ) )
            {
                return ( TExtension ) extension;
            }

            return null;
        }

        /// <summary>
        /// Gets the extension for the given type associated with this instance. Throws
        /// an exception if extension is not found.
        /// </summary>
        /// <typeparam name="TExtension">The type of extension to retrieve.</typeparam>
        /// <returns>An instance of <typeparamref name="TExtension"/>.</returns>
        public TExtension GetExtension<TExtension>()
            where TExtension : class
        {
            var extension = FindExtension<TExtension>();

            if ( extension == null )
            {
                throw new ExtensionNotFoundException( typeof( TExtension ) );
            }

            return extension;
        }

        /// <summary>
        /// Adds or replaces an extension.
        /// </summary>
        /// <typeparam name="TExtension">The type of extension to be stored.</typeparam>
        /// <param name="extension">The extension instance.</param>
        public void AddOrReplaceExtension<TExtension>( TExtension extension )
            where TExtension : class
        {
            _extensions[typeof( TExtension )] = extension;
        }

        #endregion

        #region Validator

        /// <summary>
        /// Provides basic validation of an <see cref="Entity"/> object.
        /// </summary>
        private class EntityValidator : AbstractValidator<Entity>
        {
            /// <summary>
            /// Creates a new instance of the <see cref="EntityValidator"/> class.
            /// </summary>
            public EntityValidator()
            {
                RuleFor( a => a.Guid ).NotEmpty();
            }
        }

        #endregion
    }
}
