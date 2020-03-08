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

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Provides additional functionality to the <see cref="IEnumerable{T}"/> class.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Sorts a collection by ensuring that any dependencies happen first.
        /// </summary>
        /// <typeparam name="T">The type to be sorted.</typeparam>
        /// <param name="source">The collection to be sorted.</param>
        /// <param name="dependencies">The function to call to determine dependencies of an item.</param>
        /// <returns>A sorted array that ensures all dependencies come before the items that depend on them.</returns>
        /// <remarks>
        /// https://stackoverflow.com/a/24058279
        /// </remarks>
        public static IEnumerable<T> TopologicalSort<T>( this IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies )
        {
            var elems = source.ToDictionary( node => node, node => new HashSet<T>( dependencies( node ) ) );

            while ( elems.Count > 0 )
            {
                var elem = elems.FirstOrDefault( x => x.Value.Count == 0 );
                if ( elem.Key == null )
                {
                    throw new ArgumentException( "Cyclic connections are not allowed" );
                }

                elems.Remove( elem.Key );

                foreach ( var selem in elems )
                {
                    selem.Value.Remove( elem.Key );
                }

                yield return elem.Key;
            }
        }
    }
}
