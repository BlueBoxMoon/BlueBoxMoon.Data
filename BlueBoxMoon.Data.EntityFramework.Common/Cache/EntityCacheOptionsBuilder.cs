﻿// MIT License
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

using BlueBoxMoon.Data.EntityFramework.Common.Cache.Internals;

namespace BlueBoxMoon.Data.EntityFramework.Common.Cache
{
    /// <summary>
    /// Initializes the Cache Plugin options.
    /// </summary>
    public class EntityCacheOptionsBuilder
    {
        #region Fields

        /// <summary>
        /// Reference to the options.
        /// </summary>
        private EntityCacheOptions _options;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="EntityCacheOptionsBuilder"/> class.
        /// </summary>
        /// <param name="options">The options to be built.</param>
        internal EntityCacheOptionsBuilder( EntityCacheOptions options )
        {
            _options = options;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Configures an entity type to be available for caching.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to be configured.</typeparam>
        /// <typeparam name="TCached">The cached type to be used.</typeparam>
        /// <returns>An <see cref="EntityCacheOptionsBuilder"/> that can be used to further configure options.</returns>
        public EntityCacheOptionsBuilder WithCachedType<TEntity, TCached>()
            where TEntity : class, IEntity, new()
            where TCached : class, ICachedEntity, new()
        {
            var cachedDataSetType = typeof( CachedDataSet<,> ).MakeGenericType( typeof( TEntity ), typeof( TCached ) );

            var lookup = new CachedTypeLookup( typeof( TEntity ), typeof( TCached ), cachedDataSetType );

            if ( !_options.CachedTypesByCachedEntity.ContainsKey( typeof( TCached ) ) )
            {
                _options.CachedTypesByCachedEntity.Add( typeof( TCached ), lookup );
            }

            return this;
        }

        #endregion
    }
}