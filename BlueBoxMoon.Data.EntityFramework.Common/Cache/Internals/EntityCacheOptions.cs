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

using Microsoft.Extensions.Caching.Memory;

namespace BlueBoxMoon.Data.EntityFramework.Common.Cache.Internals
{
    /// <summary>
    /// The options used at runtime for the Entity Cache plugin.
    /// </summary>
    internal class EntityCacheOptions
    {
        /// <summary>
        /// The cached types by CachedEntty type key.
        /// </summary>
        public Dictionary<Type, CachedTypeLookup> CachedTypesByCachedEntity { get; } = new Dictionary<Type, CachedTypeLookup>();

        /// <summary>
        /// The memory cache to use.
        /// </summary>
        public IMemoryCache MemoryCache { get; internal set; }
    }
}
