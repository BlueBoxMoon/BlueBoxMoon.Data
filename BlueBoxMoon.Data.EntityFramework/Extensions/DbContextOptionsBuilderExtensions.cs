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

using BlueBoxMoon.Data.EntityFramework.Internals;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueBoxMoon.Data.EntityFramework
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Configures the database context to use <see cref="EntityDbContext"/> options.
        /// </summary>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <param name="entityOptionsAction">An optional action to allow additional configuration.</param>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static DbContextOptionsBuilder UseEntityDbContext(
            this DbContextOptionsBuilder optionsBuilder,
            Action<EntityDbContextOptionsBuilder> entityOptionsAction = null )
        {
            var extension = optionsBuilder.Options.FindExtension<EntityDbContextOptionsExtension>()
                ?? new EntityDbContextOptionsExtension();

            ( ( IDbContextOptionsBuilderInfrastructure ) optionsBuilder ).AddOrUpdateExtension( extension );

            optionsBuilder.ReplaceService<IMigrationsAssembly, EntityMigrationsAssembly>();
            entityOptionsAction?.Invoke( extension.Builder );

            return optionsBuilder;
        }

        /// <summary>
        /// Configures the database context to use <see cref="EntityDbContext"/> options.
        /// </summary>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <param name="entityOptionsAction">An optional action to allow additional configuration.</param>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static DbContextOptionsBuilder<TContext> UseEntityDbContext<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder,
            Action<EntityDbContextOptionsBuilder> entityOptionsAction = null )
            where TContext : EntityDbContext
            => ( DbContextOptionsBuilder<TContext> ) UseEntityDbContext( ( DbContextOptionsBuilder ) optionsBuilder, entityOptionsAction );
    }
}
