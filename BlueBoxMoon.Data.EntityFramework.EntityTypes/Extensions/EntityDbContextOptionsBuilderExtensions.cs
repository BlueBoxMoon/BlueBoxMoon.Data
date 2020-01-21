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
using BlueBoxMoon.Data.EntityFramework.EntityTypes.Internals;

namespace BlueBoxMoon.Data.EntityFramework.EntityTypes
{
    /// <summary>
    /// Extension methods for the <see cref="EntityDbContextOptionsBuilder"/> class.
    /// </summary>
    public static class EntityDbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Enables use of entity types for this entity database context.
        /// </summary>
        /// <param name="entityOptionsBuilder">The entity options builder.</param>
        /// <param name="optionsAction">The action to call for configuring cache options.</param>
        /// <returns>The options builder so that further configuration can be chained.</returns>
        public static EntityDbContextOptionsBuilder UseEntityTypes(
            this EntityDbContextOptionsBuilder entityOptionsBuilder,
            Action<EntityTypesOptionsBuilder> optionsAction = null )
        {
            //
            // Ensure the cache system has been initialized and register our
            // cached types.
            //
            entityOptionsBuilder.UseEntityCache( a =>
            {
                a.WithCachedType<EntityType, CachedEntityType>();
            } );

            //
            // Register our custom options.
            //
            var options = entityOptionsBuilder.FindExtension<EntityTypesOptions>() ?? new EntityTypesOptions();
            entityOptionsBuilder.AddOrReplaceExtension( options );

            //
            // Register ourselves and any entities we have.
            //
            entityOptionsBuilder.WithPlugin<EntityTypesPlugin>();
            entityOptionsBuilder.WithEntity<EntityType, DataSet<EntityType>>();

            //
            // Allow the caller to customize our options.
            //
            var optionsBuilder = new EntityTypesOptionsBuilder( options );
            optionsAction?.Invoke( optionsBuilder );

            return entityOptionsBuilder;
        }
    }
}
