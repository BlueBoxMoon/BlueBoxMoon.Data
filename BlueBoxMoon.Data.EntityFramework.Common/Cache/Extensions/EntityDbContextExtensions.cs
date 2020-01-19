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

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BlueBoxMoon.Data.EntityFramework
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
    }
}
