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
using BlueBoxMoon.Data.EntityFramework.Internals;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace BlueBoxMoon.Data.EntityFramework
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Configures the database context to use <see cref="EntityDbContext"/> options.
        /// </summary>
        /// <typeparam name="TContext">The database context type to add.</typeparam>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <param name="optionsAction">The builder being used to configure the database context.</param>
        /// <param name="entityOptionsAction">An optional action to allow additional configuration of the entity context.</param>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static IServiceCollection AddEntityDbContext<TContext>(
            this IServiceCollection serviceCollection,
            Action<DbContextOptionsBuilder> optionsAction = null,
            Action<EntityDbContextOptionsBuilder> entityOptionsAction = null )
            where TContext : EntityDbContext
        {
            serviceCollection.AddSingleton<IDbContextFactory<TContext>, DbContextFactory<TContext>>();

            serviceCollection.AddDbContext<TContext>( optionsBuilder =>
            {
                var extension = optionsBuilder.Options.FindExtension<EntityDbContextOptionsExtension>()
                    ?? new EntityDbContextOptionsExtension( optionsBuilder ) { ApplicationServiceCollection = serviceCollection };

                ( ( IDbContextOptionsBuilderInfrastructure ) optionsBuilder ).AddOrUpdateExtension( extension );

                optionsBuilder.ReplaceService<IMigrationsAssembly, EntityMigrationsAssembly>();

                optionsAction?.Invoke( optionsBuilder );
                entityOptionsAction?.Invoke( extension.Builder );
            } );

            return serviceCollection;
        }

        /// <summary>
        /// Configures the database context to use <see cref="EntityDbContext"/> options.
        /// </summary>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <param name="entityOptionsAction">An optional action to allow additional configuration of the entity context.</param>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static IServiceCollection AddEntityDbContext<TContext>(
            this IServiceCollection serviceCollection,
            Action<EntityDbContextOptionsBuilder> entityOptionsAction = null )
            where TContext : EntityDbContext
            => AddEntityDbContext<TContext>( serviceCollection, null, entityOptionsAction );
    }
}
