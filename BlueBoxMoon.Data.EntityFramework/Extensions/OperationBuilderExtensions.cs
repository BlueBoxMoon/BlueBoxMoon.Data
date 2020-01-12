using System;

using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Extensions for the OperationBuilder class.
    /// </summary>
    public static class OperationBuilderExtensions
    {
        /// <summary>
        /// Defines a column as auto incrementing.
        /// </summary>
        /// <param name="operationBuilder">The operation builder which defines the column.</param>
        /// <param name="provider">The database provider.</param>
        /// <returns></returns>
        public static OperationBuilder<AddColumnOperation> AutoIncrement( this OperationBuilder<AddColumnOperation> operationBuilder, MigrationBuilder migrationBuilder )
        {
            if ( migrationBuilder is ModelMigrationBuilder modelMigrationBuilder )
            {
                modelMigrationBuilder.DatabaseFeatures.AutoIncrementColumn( operationBuilder );
            }
            else
            {
                throw new NotSupportedException( $"Provider '{migrationBuilder.ActiveProvider}' is not supported." );
            }

            return operationBuilder;
        }
    }
}
