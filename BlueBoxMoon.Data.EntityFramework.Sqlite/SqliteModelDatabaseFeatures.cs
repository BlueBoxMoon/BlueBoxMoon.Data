using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace BlueBoxMoon.Data.EntityFramework.Sqlite
{
    internal class SqliteModelDatabaseFeatures : IModelDatabaseFeatures
    {
        public void AutoIncrementColumn( OperationBuilder<AddColumnOperation> operation )
        {
            operation.Annotation( "Sqlite:Autoincrement", true );
        }
    }
}
