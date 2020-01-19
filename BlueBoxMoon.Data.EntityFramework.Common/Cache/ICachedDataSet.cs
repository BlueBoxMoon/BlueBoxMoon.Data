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

namespace BlueBoxMoon.Data.EntityFramework.Common.Cache
{
    /// <summary>
    /// Specifies the requirements of a cached data set.
    /// </summary>
    /// <typeparam name="TCached"></typeparam>
    public interface ICachedDataSet<TCached>
        where TCached : class, ICachedEntity, new()
    {
        /// <summary>
        /// Gets all <typeparamref name="TCached"/> items, all items are loaded
        /// into cache if they are not already in cache.
        /// </summary>
        /// <returns>An enumerable of <typeparamref name="TCached"/> instances.</returns>
        IEnumerable<TCached> GetAll();

        /// <summary>
        /// Gets a cached entity by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the item to be retrieved.</param>
        /// <returns>The cached entity or <c>null</c> if it was not found.</returns>
        TCached GetById( long id );

        /// <summary>
        /// Gets a cached entity by its unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier of the item to be retrieved.</param>
        /// <returns>The cached entity or <c>null</c> if it was not found.</returns>
        TCached GetByGuid( Guid guid );

        /// <summary>
        /// Removes an item from the cache based on its Id number.
        /// </summary>
        /// <param name="id">The identifier of the item to be removed.</param>
        void Remove( long id );
    }
}
