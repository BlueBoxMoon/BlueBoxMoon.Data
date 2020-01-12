using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;

namespace BlueBoxMoon.Data.EntityFramework
{
    public abstract class ModelMigration : Migration
    {
        internal protected IServiceProvider ServiceProvider { get; internal set; }

        protected IModelDatabaseFeatures DatabaseFeatures => ServiceProvider.GetService<IModelDatabaseFeatures>();

        public override IReadOnlyList<MigrationOperation> UpOperations
        {
            get
            {
                if ( _upOperations == null )
                {
                    _upOperations = BuildOperations( Up );
                }

                return _upOperations;
            }
        }
        private List<MigrationOperation> _upOperations;

        public override IReadOnlyList<MigrationOperation> DownOperations
        {
            get
            {
                if ( _downOperations == null )
                {
                    _downOperations = BuildOperations( Down );
                }

                return _downOperations;
            }
        }
        private List<MigrationOperation> _downOperations;

        private List<MigrationOperation> BuildOperations( Action<MigrationBuilder> action )
        {
            var migrationBuilder = new ModelMigrationBuilder( ActiveProvider, DatabaseFeatures );

            action( migrationBuilder );

            return migrationBuilder.Operations;
        }
    }
}
