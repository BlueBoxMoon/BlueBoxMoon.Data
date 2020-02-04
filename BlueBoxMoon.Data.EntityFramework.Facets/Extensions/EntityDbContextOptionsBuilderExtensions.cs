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

using BlueBoxMoon.Data.EntityFramework.Facets.Internals;
using BlueBoxMoon.Data.EntityFramework.Cache;

namespace BlueBoxMoon.Data.EntityFramework.Facets
{
    /// <summary>
    /// Extension methods for the <see cref="EntityDbContextOptionsBuilder"/> class.
    /// </summary>
    public static class EntityDbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Enables use of facets for this entity database context.
        /// </summary>
        /// <param name="entityOptionsBuilder">The entity options builder.</param>
        /// <param name="optionsAction">The action to call for configuring cache options.</param>
        /// <returns>The options builder so that further configuration can be chained.</returns>
        public static EntityDbContextOptionsBuilder UseFacets(
            this EntityDbContextOptionsBuilder entityOptionsBuilder,
            Action<FacetsOptionsBuilder> optionsAction = null )
        {
            //
            // Ensure the cache system has been initialized and register our
            // cached types.
            //
            entityOptionsBuilder.UseEntityCache( a =>
            {
                a.WithCachedType<Facet, CachedFacet>();
            } );

            //
            // Register our custom options.
            //
            var options = entityOptionsBuilder.FindExtension<FacetOptions>() ?? new FacetOptions();
            entityOptionsBuilder.AddOrReplaceExtension( options );

            //
            // Register ourselves and any entities we have.
            //
            entityOptionsBuilder.WithPlugin<FacetsPlugin>();
            entityOptionsBuilder.WithEntity<Facet, DataSet<Facet>>();
            entityOptionsBuilder.WithEntity<FacetValue, DataSet<FacetValue>>();

            //
            // Allow the caller to customize our options.
            //
            var optionsBuilder = new FacetsOptionsBuilder( options );
            optionsAction?.Invoke( optionsBuilder );

            return entityOptionsBuilder;
        }
    }
}
