
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueBoxMoon.Data.EntityFramework
{
    public class ModelMigrationBuilder : MigrationBuilder
    {
        internal IModelDatabaseFeatures DatabaseFeatures { get; }

        public ModelMigrationBuilder( string activeProvider, IModelDatabaseFeatures databaseFeatures )
            : base( activeProvider )
        {
            DatabaseFeatures = databaseFeatures;
        }
    }
}
