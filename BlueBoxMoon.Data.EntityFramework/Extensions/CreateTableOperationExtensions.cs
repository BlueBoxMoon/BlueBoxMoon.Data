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
