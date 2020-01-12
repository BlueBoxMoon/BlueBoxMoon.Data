using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace BlueBoxMoon.Data.EntityFramework
{
    public interface IModelDatabaseFeatures
    {
        void AutoIncrementColumn( OperationBuilder<AddColumnOperation> operation );
    }
}
