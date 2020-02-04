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

using BlueBoxMoon.Data.EntityFramework.Facets.Internals;
using BlueBoxMoon.Data.EntityFramework.Cache;
using BlueBoxMoon.Data.EntityFramework.EntityTypes;

using Microsoft.EntityFrameworkCore;

namespace BlueBoxMoon.Data.EntityFramework.Facets
{
    /// <summary>
    /// Extension methods for the <see cref="IEntity"/> interface.
    /// </summary>
    public static class IEntityExtensions
    {
        public static void LoadFacetValues<TEntity>( this TEntity entity, EntityDbContext entityDbContext )
            where TEntity : IEntity, IExtensible
        {
            var FacetData = entity.FindExtension<EntityFacetData>();

            if ( FacetData != null )
            {
                return;
            }

            var cachedEntityTypeSet = entityDbContext.GetCachedDataSet<CachedEntityType>();
            var cachedFacetset = entityDbContext.GetCachedDataSet<CachedFacet>();
            var FacetValueSet = entityDbContext.GetDataSet<FacetValue>();

            CachedEntityType entityType = cachedEntityTypeSet.GetByType<TEntity>();

            var Facets = cachedFacetset.GetAll()
                .Where( a => a.EntityTypeId == entityType.Id )
                .Where( a => a.DoQualifiersMatchForEntity( entity ) )
                .ToList();

            var FacetIds = Facets.Select( a => a.Id )
                .ToList();
            var FacetValues = FacetValueSet.Where( a => FacetIds.Contains( a.FacetId ) )
                .ToList();

            var values = Facets.ToDictionary(
                a => a,
                a => FacetValues.FirstOrDefault( b => b.FacetId == a.Id ) ?? FacetValueSet.Create() );

            FacetData = new EntityFacetData( values );

            entity.AddOrReplaceExtension( FacetData );
        }

        public static void SaveFacetValues<TEntity>( this TEntity entity, EntityDbContext entityDbContext )
            where TEntity : IEntity, IExtensible
        {
            var FacetData = entity.FindExtension<EntityFacetData>();

            if ( FacetData == null )
            {
                return;
            }

            var FacetValueSet = entityDbContext.GetDataSet<FacetValue>();

            foreach ( var key in FacetData.FacetValues.Keys.ToList() )
            {
                var FacetValue = FacetData.FacetValues[key];

                if ( FacetValue.Id == 0 && FacetValue.Value != null )
                {
                    FacetValue.FacetId = key.Id;
                    FacetValue.EntityId = entity.Id;

                    FacetValueSet.Add( FacetValue );
                }
            }

            entityDbContext.SaveChanges();
        }

        public static string GetFacetValue<TEntity>( this TEntity entity, string FacetKey )
            where TEntity : IEntity, IExtensible
        {
            var FacetData = entity.FindExtension<EntityFacetData>();

            if ( FacetData == null )
            {
                return null;
            }

            var kvp = FacetData.FacetValues
                .Where( a => a.Key.Key == FacetKey )
                .FirstOrDefault();

            if ( kvp.Key == null )
            {
                return null;
            }

            return kvp.Value?.Value ?? kvp.Key.DefaultValue;
        }

        public static string GetFacetValue<TEntity>( this TEntity  entity, Guid FacetGuid )
            where TEntity : IEntity, IExtensible
        {
            var FacetData = entity.FindExtension<EntityFacetData>();

            if ( FacetData == null )
            {
                return null;
            }

            var kvp = FacetData.FacetValues
                .Where( a => a.Key.Guid == FacetGuid )
                .FirstOrDefault();

            if ( kvp.Key == null )
            {
                return null;
            }

            return kvp.Value?.Value ?? kvp.Key.DefaultValue;
        }

        public static void SetFacetValue<TEntity>( this TEntity entity, string FacetKey, string value )
            where TEntity : IEntity, IExtensible
        {
            var FacetData = entity.FindExtension<EntityFacetData>();

            if ( FacetData == null )
            {
                return;
            }

            var Facet = FacetData.FacetValues
                .Keys
                .Where( a => a.Key == FacetKey )
                .FirstOrDefault();

            if ( Facet == null )
            {
                return;
            }

            FacetData.FacetValues[Facet].Value = value;
        }

        public static void SetFacetValue<TEntity>( this TEntity entity, Guid FacetGuid, string value )
            where TEntity : IEntity, IExtensible
        {
            var FacetData = entity.FindExtension<EntityFacetData>();

            if ( FacetData == null )
            {
                return;
            }

            var Facet = FacetData.FacetValues
                .Keys
                .Where( a => a.Guid == FacetGuid )
                .FirstOrDefault();

            if ( Facet == null )
            {
                return;
            }

            FacetData.FacetValues[Facet].Value = value;
        }
    }

    internal class EntityFacetData
    {
        public Dictionary<CachedFacet, FacetValue> FacetValues { get; }

        public EntityFacetData( Dictionary<CachedFacet, FacetValue> values )
        {
            FacetValues = values;
        }
    }
}
