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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlueBoxMoon.Data.EntityFramework
{
    public static class ModelDbServiceCollectionExtensions
    {
        //public static IServiceCollection AddModelDbContext<TContext>(
        //    this IServiceCollection serviceCollection,
        //    Action<DbContextOptionsBuilder> optionsAction = null,
        //    ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
        //    ServiceLifetime optionsLifetime = ServiceLifetime.Scoped )
        //    where TContext : ModelDbContext
        //    => AddModelDbContext<TContext, TContext>( serviceCollection, optionsAction, contextLifetime, optionsLifetime );

        //public static IServiceCollection AddModelDbContext<TContextService, TContextImplementation>(
        //    this IServiceCollection serviceCollection,
        //    Action<DbContextOptionsBuilder> optionsAction = null,
        //    ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
        //    ServiceLifetime optionsLifetime = ServiceLifetime.Scoped )
        //    where TContextImplementation : ModelDbContext, TContextService
        //    => AddModelDbContext<TContextService, TContextImplementation>(
        //        serviceCollection,
        //        optionsAction == null
        //            ? (Action<IServiceProvider, DbContextOptionsBuilder>)null
        //            : (p, b) => optionsAction(b),
        //        contextLifetime, optionsLifetime );
    }
}
