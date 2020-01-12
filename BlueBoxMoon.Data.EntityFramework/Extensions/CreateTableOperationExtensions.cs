using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace BlueBoxMoon.Data.EntityFramework
{
    public static class CreateTableOperationExtensions
    {
        public static CreateTableBuilder<TColumns> AppendColumns<TColumns>( this CreateTableOperation createTableOperation, Func<ColumnsBuilder, TColumns> columns, Action<CreateTableBuilder<TColumns>> constraints = null )
        {
            var columnsBuilder = new ColumnsBuilder( createTableOperation );

            var columnsObject = columns( columnsBuilder );
            var columnMap = new Dictionary<PropertyInfo, AddColumnOperation>();

            foreach ( var property in typeof( TColumns ).GetTypeInfo().DeclaredProperties )
            {
                var addColumnOperation = ( ( IInfrastructure<AddColumnOperation> ) property.GetMethod.Invoke( columnsObject, null ) ).Instance;
                if ( addColumnOperation.Name == null )
                {
                    addColumnOperation.Name = property.Name;
                }

                columnMap.Add( property, addColumnOperation );
            }

            var builder = new CreateTableBuilder<TColumns>( createTableOperation, columnMap );

            constraints?.Invoke( builder );

            return builder;
        }
    }
}
