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
