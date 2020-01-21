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

namespace BlueBoxMoon.Data.EntityFramework.Cache.Internals
{
    /// <summary>
    /// Provides initialization information about cached types.
    /// </summary>
    internal class CachedTypeLookup
    {
        /// <summary>
        /// The primary <see cref="Entity"/> type.
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// The cached <see cref="CachedEntity"/> entity.
        /// </summary>
        public Type CachedType { get; }

        /// <summary>
        /// The <see cref="CachedDataSet{TEntity, TCached}"/> that will provide
        /// cached lookup functionality.
        /// </summary>
        public Type CachedDataSetType { get; }

        /// <summary>
        /// Create a new instance of the <see cref="CachedTypeLookup"/> class.
        /// </summary>
        /// <param name="entityType">The entity type to be cached.</param>
        /// <param name="cachedType">The cached type that will provide details about <paramref name="entityType"/>.</param>
        /// <param name="cachedDataSetType">The cached data set provider.</param>
        public CachedTypeLookup( Type entityType, Type cachedType, Type cachedDataSetType )
        {
            EntityType = entityType;
            CachedType = cachedType;
            CachedDataSetType = cachedDataSetType;
        }
    }
}
