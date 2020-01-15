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

using BlueBoxMoon.Data.EntityFramework.Infrastructure;

using Microsoft.Extensions.DependencyInjection;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Creates a new transient <typeparamref name="TContext"/> instance.
    /// </summary>
    /// <typeparam name="TContext">The <see cref="EntityDbContext"/> type to instantiate.</typeparam>
    public class DbContextFactory<TContext> : IDbContextFactory<TContext>
        where TContext : EntityDbContext
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Instantiates a new instance of the <see cref="DbContextFactory{TContext}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The current service provider.</param>
        /// <param name="applicationServiceFinder">A way to find the parent service provider.</param>
        public DbContextFactory( IServiceProvider serviceProvider, IParentServiceProvider applicationServiceFinder = null )
        {
            _serviceProvider = applicationServiceFinder?.ServiceProvider ?? serviceProvider;
        }

        /// <summary>
        /// Creates a new transient instance of <typeparamref name="TContext"/>.
        /// </summary>
        /// <returns>An instance of <typeparamref name="TContext"/>.</returns>
        public TContext CreateContext()
        {
            return ActivatorUtilities.CreateInstance<TContext>( _serviceProvider );
        }
    }
}
