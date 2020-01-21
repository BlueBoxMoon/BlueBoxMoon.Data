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

using BlueBoxMoon.Data.EntityFramework.Cache.Internals;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BlueBoxMoon.Data.EntityFramework.Cache
{
    /// <summary>
    /// Extension methods for the <see cref="EntityDbContext"/> class.
    /// </summary>
    public static class EntityDbContextExtensions
    {
        /// <summary>
        /// Gets the <see cref="ICachedDataSet{TCached}"/> instance that can be used to
        /// interact with the cache.
        /// </summary>
        /// <typeparam name="TCached">The <see cref="ICachedEntity"/> type.</typeparam>
        /// <param name="entityDbContext">The <see cref="EntityDbContext"/> controlling database access.</param>
        /// <returns>An instance of <see cref="ICachedDataSet{TCached}"/> or <c>null</c> if no implementation found.</returns>
        public static ICachedDataSet<TCached> GetCachedDataSet<TCached>(
            this EntityDbContext entityDbContext )
            where TCached : class, ICachedEntity, new()
        {
            var serviceProvider = ( ( IInfrastructure<IServiceProvider> ) entityDbContext ).Instance;

            return serviceProvider.GetService<ICachedDataSet<TCached>>();
        }

        /// <summary>
        /// Gets the <see cref="ICachedDataSet{TCached}"/> instance that can be used to
        /// interact with the cache.
        /// </summary>
        /// <param name="entityDbContext">The <see cref="EntityDbContext"/> controlling database access.</param>
        /// <param name="cachedType">The cached type whose dataset is to be retrieved.</param>
        /// <returns>An instance of <see cref="ICachedDataSet{TCached}"/> or <c>null</c> if no implementation found.</returns>
        public static ICachedDataSet<CachedEntity> GetCachedDataSet(
            this EntityDbContext entityDbContext,
            Type cachedType )
        {
            if ( !typeof( ICachedEntity ).IsAssignableFrom( cachedType ) )
            {
                throw new ArgumentException( $"Type must inherit from {nameof( ICachedEntity )}", nameof( cachedType ) );
            }

            var lookup = entityDbContext.EntityContextOptions
                .GetExtension<EntityCacheOptions>()
                .CachedTypesByCachedEntity[cachedType];

            var serviceProvider = ( ( IInfrastructure<IServiceProvider> ) entityDbContext ).Instance;

            var iCachedDataSetType = typeof( ICachedDataSet<> ).MakeGenericType( lookup.CachedType );

            return ( ICachedDataSet<CachedEntity> ) serviceProvider.GetService( iCachedDataSetType );
        }
    }
}
