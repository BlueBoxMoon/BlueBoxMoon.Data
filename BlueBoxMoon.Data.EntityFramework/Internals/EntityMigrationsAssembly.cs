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
using System.Reflection;

using BlueBoxMoon.Data.EntityFramework.Migrations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace BlueBoxMoon.Data.EntityFramework.Internals
{
#pragma warning disable EF1001
    /// <summary>
    /// Custom migration assembly resolver to pass in the service provider
    /// to the migrations.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationsAssembly" />
    public class EntityMigrationsAssembly : MigrationsAssembly
    {
        #region Fields

        private readonly EntityDbContext _dbContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMigrationsAssembly"/> class.
        /// </summary>
        /// <param name="currentContext"></param>
        /// <param name="options"></param>
        /// <param name="idGenerator"></param>
        /// <param name="logger"></param>
        public EntityMigrationsAssembly(
            ICurrentDbContext currentContext,
            IDbContextOptions options,
            IMigrationsIdGenerator idGenerator,
            IDiagnosticsLogger<DbLoggerCategory.Migrations> logger )
            : base( currentContext, options, idGenerator, logger )
        {
            _dbContext = ( EntityDbContext ) currentContext.Context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        /// the same compatibility standards as public APIs. It may be changed or removed without notice in
        /// any release. You should only use it directly in your code with extreme caution and knowing that
        /// doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        /// <param name="migrationClass"></param>
        /// <param name="activeProvider"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">activeProvider</exception>
        public override Migration CreateMigration( TypeInfo migrationClass, string activeProvider )
        {
            if ( activeProvider == null )
            {
                throw new ArgumentNullException( nameof( activeProvider ) );
            }

            var serviceProvider = _dbContext.GetInfrastructure();

            var migration = ( Migration ) ActivatorUtilities.CreateInstance( serviceProvider, migrationClass.AsType() );

            migration.ActiveProvider = activeProvider;

            if ( migration is EntityMigration entityMigration )
            {
                entityMigration.ServiceProvider = serviceProvider;
            }

            return migration;
        }

        #endregion
    }
#pragma warning restore EF1001
}
