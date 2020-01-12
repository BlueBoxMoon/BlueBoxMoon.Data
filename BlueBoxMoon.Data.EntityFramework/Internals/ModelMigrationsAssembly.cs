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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace BlueBoxMoon.Data.EntityFramework.Internals
{
#pragma warning disable EF1001
    public class ModelMigrationsAssembly : MigrationsAssembly
    {
        private readonly ModelDbContext _dbContext;

        public ModelMigrationsAssembly(
            ICurrentDbContext currentContext,
            IDbContextOptions options,
            IMigrationsIdGenerator idGenerator,
            IDiagnosticsLogger<DbLoggerCategory.Migrations> logger )
            : base( currentContext, options, idGenerator, logger )
        {
            _dbContext = ( ModelDbContext ) currentContext.Context;
        }

        public override Migration CreateMigration( TypeInfo migrationClass, string activeProvider )
        {
            if ( activeProvider == null )
            {
                throw new ArgumentNullException( nameof( activeProvider ) );
            }

            var serviceProvider = _dbContext.GetInfrastructure();

            var migration = ( Migration ) ActivatorUtilities.CreateInstance( serviceProvider, migrationClass.AsType() );

            migration.ActiveProvider = activeProvider;

            if ( migration is ModelMigration modelMigration )
            {
                modelMigration.ServiceProvider = serviceProvider;
            }

            return migration;
        }
    }
#pragma warning restore EF1001
}
