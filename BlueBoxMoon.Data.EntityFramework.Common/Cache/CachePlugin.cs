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

using BlueBoxMoon.Data.EntityFramework.Common.Cache.Internals;

using Microsoft.Extensions.DependencyInjection;

namespace BlueBoxMoon.Data.EntityFramework.Common.Cache
{
    /// <summary>
    /// Defines the requirements of the Entity Cache plugin.
    /// </summary>
    public class CachePlugin : EntityPlugin
    {
        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        /// <value>
        /// The name of the plugin.
        /// </value>
        public override string Name => "Entity Cache";

        /// <summary>
        /// Get the migration types that need to be run to install or uninstall
        /// this plugin.
        /// </summary>
        /// <returns>An enumerable of <see cref="Type"/>.</returns>
        public override IEnumerable<Type> GetMigrations()
        {
            return new List<Type>();
        }

        /// <summary>
        /// Called when the services are being registered in the DbContext.
        /// </summary>
        /// <param name="serviceCollection">The service collection that can have additional services registered in.</param>
        /// <param name="entityDbContextOptions">The <see cref="EntityDbContextOptions"/> for this context.</param>
        public override void ApplyServices( IServiceCollection serviceCollection, EntityDbContextOptions entityDbContextOptions )
        {
            var options = entityDbContextOptions.GetExtension<EntityCacheOptions>();
            var genericCachedDataSetType = typeof( ICachedDataSet<> );

            foreach ( var lookup in options.CachedTypesByCachedEntity.Values )
            {
                serviceCollection.AddScoped( genericCachedDataSetType.MakeGenericType( lookup.CachedType ), lookup.CachedDataSetType );
            }
        }
    }
}
