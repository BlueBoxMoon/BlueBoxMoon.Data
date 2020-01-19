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

using BlueBoxMoon.Data.EntityFramework.Common.Cache;
using BlueBoxMoon.Data.EntityFramework.Common.Cache.Internals;

using Microsoft.Extensions.Caching.Memory;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Extension methods for the <see cref="EntityDbContextOptionsBuilder"/> class.
    /// </summary>
    public static class EntityDbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Enables use of caching for this entity database context.
        /// </summary>
        /// <param name="entityOptionsBuilder">The entity options builder.</param>
        /// <param name="optionsAction">The action to call for configuring cache options.</param>
        /// <returns>The options builder so that further configuration can be chained.</returns>
        public static EntityDbContextOptionsBuilder UseEntityCache(
            this EntityDbContextOptionsBuilder entityOptionsBuilder,
            Action<EntityCacheOptionsBuilder> optionsAction = null )
        {
            var options = entityOptionsBuilder.FindExtension<EntityCacheOptions>() ?? new EntityCacheOptions();

            if ( options.MemoryCache == null )
            {
                var memoryOptions = Microsoft.Extensions.Options.Options.Create( new MemoryCacheOptions() );
                options.MemoryCache = new MemoryCache( memoryOptions );
            }

            entityOptionsBuilder.AddOrReplaceExtension( options );
            entityOptionsBuilder.WithPlugin<CachePlugin>();

            var optionsBuilder = new EntityCacheOptionsBuilder( options );
            optionsAction?.Invoke( optionsBuilder );

            return entityOptionsBuilder;
        }
    }
}
