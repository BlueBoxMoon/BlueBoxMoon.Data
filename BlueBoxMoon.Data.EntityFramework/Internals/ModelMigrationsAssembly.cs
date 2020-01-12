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
