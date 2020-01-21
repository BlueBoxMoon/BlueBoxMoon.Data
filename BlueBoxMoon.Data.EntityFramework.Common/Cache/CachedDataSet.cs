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
using System.Linq;

using BlueBoxMoon.Data.EntityFramework.Common.Cache.Internals;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;

namespace BlueBoxMoon.Data.EntityFramework.Common.Cache
{
    /// <summary>
    /// A cached dataset handles interacting with the cache on behalf of
    /// the application.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be cached.</typeparam>
    /// <typeparam name="TCached">The cached entity type that represents <typeparamref name="TEntity"/>.</typeparam>
    public class CachedDataSet<TEntity, TCached> : ICachedDataSet<TCached>
        where TEntity : class, IEntity, new()
        where TCached : class, ICachedEntity, new()
    {
        #region Fields

        private IMemoryCache _cache;

        #endregion

        #region Properties

        /// <summary>
        /// The base cache key used for this entity type.
        /// </summary>
        protected virtual string BaseCacheKey { get; set; }

        /// <summary>
        /// Gets the entity database context associated with this cached data
        /// set.
        /// </summary>
        protected EntityDbContext DbContext { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="CachedDataSet{TEntity, TCached}"/>
        /// class.
        /// </summary>
        /// <param name="dbContext">The entity database context.</param>
        public CachedDataSet( ICurrentDbContext dbContext )
        {
            DbContext = ( EntityDbContext ) dbContext.Context;
            _cache = DbContext.EntityContextOptions.GetExtension<EntityCacheOptions>().MemoryCache;
            BaseCacheKey = typeof( TCached ).FullName;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all <typeparamref name="TCached"/> items, all items are loaded
        /// into cache if they are not already in cache.
        /// </summary>
        /// <returns>An enumerable of <typeparamref name="TCached"/> instances.</returns>
        public IEnumerable<TCached> GetAll()
        {
            var idList = _cache.GetOrCreate( GetAllCacheKey(), entry =>
            {
                entry.Size = 0;

                return DbContext.GetDataSet<TEntity>()
                    .Select( a => a.Id )
                    .ToList();
            } );

            return idList.Select( a => GetById( a ) );
        }

        /// <summary>
        /// Gets a cached entity by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the item to be retrieved.</param>
        /// <returns>The cached entity or <c>null</c> if it was not found.</returns>
        public TCached GetById( long id )
        {
            if ( !_cache.TryGetValue<TCached>( GetIdCacheKey( id ), out var cached ) )
            {
                var entity = DbContext.GetDataSet<TEntity>().GetById( id );

                return AddEntityToCache( entity );
            }

            return cached;
        }

        /// <summary>
        /// Gets a cached entity by its unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier of the item to be retrieved.</param>
        /// <returns>The cached entity or <c>null</c> if it was not found.</returns>
        public TCached GetByGuid( Guid guid )
        {
            //
            // Look up the Id number from the cache.
            //
            if ( !_cache.TryGetValue<long>( GetGuidCacheKey( guid ), out var cachedId ) )
            {
                var entity = DbContext.GetDataSet<TEntity>().GetByGuid( guid );

                return AddEntityToCache( entity );
            }

            //
            // Look up the entity from the database.
            //
            if ( !_cache.TryGetValue<TCached>( GetIdCacheKey( cachedId ), out var cached ) )
            {
                var entity = DbContext.GetDataSet<TEntity>().GetByGuid( guid );

                return AddEntityToCache( entity );
            }

            return cached;
        }

        /// <summary>
        /// Removes an item from the cache based on its Id number.
        /// </summary>
        /// <param name="id">The identifier of the item to be removed.</param>
        public void Remove( long id )
        {
            if ( _cache.TryGetValue<TCached>( GetIdCacheKey( id ), out var cachedEntity ) )
            {
                _cache.Remove( GetIdCacheKey( cachedEntity.Id ) );
                _cache.Remove( GetGuidCacheKey( cachedEntity.Guid ) );
            }

            _cache.Remove( GetAllCacheKey() );
        }

        /// <summary>
        /// Called when a cachable entity has changed. This will update the cache
        /// with the new information about the entity.
        /// </summary>
        /// <param name="entity">The entity that was changed.</param>
        /// <param name="state">The state of the entity when it was saved.</param>
        public void EntityChanged( IEntity entity, EntityState state )
        {
            if ( state == EntityState.Deleted )
            {
                Remove( entity.Id );
            }
            else
            {
                if ( !_cache.TryGetValue<TCached>( GetIdCacheKey( entity.Id ), out var cached ) )
                {
                    AddEntityToCache( ( TEntity ) entity );
                }
                else
                {
                    cached.UpdateFromEntity( entity );
                }
            }
        }

        /// <summary>
        /// Gets the cache key associated with all items.
        /// </summary>
        /// <returns>The cache key string.</returns>
        protected virtual string GetAllCacheKey()
        {
            return $"{BaseCacheKey}_All";
        }

        /// <summary>
        /// Gets the cache key associated with this identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>The cache key string.</returns>
        protected virtual string GetIdCacheKey( long id )
        {
            return $"{BaseCacheKey}_{id}";
        }

        /// <summary>
        /// Gets the cache key associated with this unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier of the entity.</param>
        /// <returns>The cache key string.</returns>
        protected virtual string GetGuidCacheKey( Guid guid )
        {
            return $"{BaseCacheKey}_{guid}";
        }

        /// <summary>
        /// Adds an entity to the cache.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>The cached entity object or <c>null</c> if <paramref name="entity"/> was not valid.</returns>
        private TCached AddEntityToCache( TEntity entity )
        {
            if ( entity == null )
            {
                return null;
            }

            var cachedEntity = new TCached();
            cachedEntity.UpdateFromEntity( entity );

            return AddCacheEntity( cachedEntity );
        }

        /// <summary>
        /// Adds a cached entity to the cache.
        /// </summary>
        /// <param name="cachedEntity">The cached entity to be added.</param>
        /// <returns>The <paramref name="cachedEntity"/> object.</returns>
        private TCached AddCacheEntity( TCached cachedEntity )
        {
            using ( var entry = _cache.CreateEntry( GetIdCacheKey( cachedEntity.Id ) ) )
            {
                entry.Size = 1;
                entry.SetValue( cachedEntity );
            }

            using ( var entry = _cache.CreateEntry( GetGuidCacheKey( cachedEntity.Guid ) ) )
            {
                entry.Size = 1;
                entry.SetValue( cachedEntity.Id );
            }

            _cache.Remove( GetAllCacheKey() );

            return cachedEntity;
        }

        #endregion
    }
}
