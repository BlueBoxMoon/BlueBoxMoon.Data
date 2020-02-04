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

using BlueBoxMoon.Data.EntityFramework.Cache;

namespace BlueBoxMoon.Data.EntityFramework.Facets
{
    /// <summary>a
    /// A cached <see cref="Facet"/> that can be used instead of loading
    /// an facet from the database.
    /// </summary>
    public class CachedFacet : CachedEntity
    {
        #region Fields

        /// <summary>
        /// The qualifiers function used to filter this facet.
        /// </summary>
        private Delegate _qualifiersFunction;

        #endregion

        #region Database Properties

        /// <summary>
        /// The entity type identifier for this facet.
        /// </summary>
        public long EntityTypeId { get; private set; }

        /// <summary>
        /// The user friendly name of the facet.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// A short identifier of the facet that can be used by the user
        /// to specify the facet without having to know it's Guid value.
        /// If multiple Key values match on a specific EntityTypeId then the
        /// first match is used.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// The description of this facet that would help the user understand
        /// how to use it.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Json encoded data about how this facet should be matched to
        /// entities beyond just the EntityTypeId.
        /// </summary>
        public string Qualifiers { get; private set; }

        /// <summary>
        /// The default value if no value has been set for an entity.
        /// </summary>
        public string DefaultValue { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the cached information from the entity.
        /// </summary>
        /// <param name="entity">The database entity.</param>
        public override void UpdateFromEntity( IEntity entity )
        {
            base.UpdateFromEntity( entity );

            var facet = ( Facet ) entity;

            EntityTypeId = facet.EntityTypeId;
            Name = facet.Name;
            Key = facet.Key;
            Description = facet.Description;
            Qualifiers = facet.Qualifiers;
            DefaultValue = facet.DefaultValue;

            try
            {
                _qualifiersFunction = facet.GetQualifiers();
            }
            catch
            {
                _qualifiersFunction = null;
            }
        }

        /// <summary>
        /// Gets the qualifiers function to be used for additional filtering.
        /// </summary>
        /// <typeparam name="TEntity">The type to cast the function to.</typeparam>
        /// <returns>A function or <c>null</c>.</returns>
        public Func<TEntity, bool> GetQualifiers<TEntity>()
            where TEntity : IEntity
        {
            return _qualifiersFunction != null ? ( Func<TEntity, bool> ) _qualifiersFunction : null;
        }

        /// <summary>
        /// Checks if the qualifier predicate matches the given entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of <paramref name="entity"/>.</typeparam>
        /// <param name="entity">The entity to be checked against.</param>
        /// <returns><c>true</c> if the entity matches, otherwise <c>false</c>.</returns>
        public bool DoQualifiersMatchForEntity<TEntity>( TEntity entity )
            where TEntity : IEntity
        {
            if ( _qualifiersFunction == null )
            {
                return true;
            }

            return ( ( Func<TEntity, bool> ) _qualifiersFunction )( entity );
        }

        #endregion
    }
}
