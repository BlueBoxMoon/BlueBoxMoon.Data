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

namespace BlueBoxMoon.Data.EntityFramework.Common.Cache
{
    /// <summary>
    /// Defines the save hook for Entity Cache plugin to update all cached
    /// types when an entity is saved.
    /// </summary>
    public class CachedEntitySaveHook : IEntitySaveHook
    {
        private EntityCacheOptions _options;
        private Dictionary<IEntity, EntityState> _modifiedEntities;

        /// <summary>
        /// Called just before changes are saved to the database.
        /// </summary>
        /// <param name="entityDbContext">The database context.</param>
        public void PreSaveChanges( EntityDbContext entityDbContext )
        {
            _options = entityDbContext.EntityContextOptions.GetExtension<EntityCacheOptions>();

            var entityTypes = _options.CachedTypesByCachedEntity
                .Values
                .Select( a => a.EntityType )
                .ToList();

            _modifiedEntities = entityDbContext.ChangeTracker
                .Entries()
                .Where( a => a.Entity is IEntity && entityTypes.Contains( a.Entity.GetType() ) )
                .Where( a => a.State == EntityState.Added || a.State == EntityState.Modified || a.State == EntityState.Deleted )
                .ToDictionary( a => ( IEntity ) a.Entity, a => a.State );
        }

        /// <summary>
        /// Called just after changes are saved to the database.
        /// </summary>
        /// <param name="entityDbContext">The database context.</param>
        /// <param name="success"><c>true</c> if the save operation succeeded.</param>
        public void PostSaveChanges( EntityDbContext entityDbContext, bool success )
        {
            if ( success )
            {
                foreach ( var entry in _modifiedEntities )
                {
                    var cachedType = _options.CachedTypesByEntity[entry.Key.GetType()].CachedType;
                    var set = entityDbContext.GetCachedDataSet( cachedType );

                    if ( entry.Value == EntityState.Deleted )
                    {
                        set.Remove( entry.Key.Id );
                    }
                    else
                    {
                        set.EntityChanged( entry.Key, entry.Value );
                    }
                }
            }
        }
    }
}
