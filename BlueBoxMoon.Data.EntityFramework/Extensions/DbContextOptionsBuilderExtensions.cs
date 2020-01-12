using System;

using BlueBoxMoon.Data.EntityFramework.Internals;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueBoxMoon.Data.EntityFramework
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Configures the database context to use ModelDbContext options.
        /// </summary>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <param name="modelOptionsAction">An optional action to allow additional configuration.</param>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static DbContextOptionsBuilder UseModelDbContext(
            this DbContextOptionsBuilder optionsBuilder,
            Action<ModelDbContextOptionsBuilder> modelOptionsAction = null )
        {
            var extension = optionsBuilder.Options.FindExtension<ModelDbContextOptionsExtension>()
                ?? new ModelDbContextOptionsExtension();

            ( ( IDbContextOptionsBuilderInfrastructure ) optionsBuilder ).AddOrUpdateExtension( extension );

            optionsBuilder.ReplaceService<IMigrationsAssembly, ModelMigrationsAssembly>();
            modelOptionsAction?.Invoke( extension.Builder );

            return optionsBuilder;
        }

        /// <summary>
        /// Configures the database context to use ModelDbContext options.
        /// </summary>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <param name="modelOptionsAction">An optional action to allow additional configuration.</param>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static DbContextOptionsBuilder<TContext> UseModelDbContext<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder,
            Action<ModelDbContextOptionsBuilder> modelOptionsAction = null )
            where TContext : ModelDbContext
            => ( DbContextOptionsBuilder<TContext> ) UseModelDbContext( ( DbContextOptionsBuilder ) optionsBuilder, modelOptionsAction );
    }
}
