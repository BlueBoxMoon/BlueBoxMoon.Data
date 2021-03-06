﻿// MIT License
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
using System.Text;

using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlueBoxMoon.Data.EntityFramework.Sqlite
{
#pragma warning disable EF1001
    public class SqliteEntitySqlGenerationHelper : SqliteSqlGenerationHelper
    {
        public SqliteEntitySqlGenerationHelper( RelationalSqlGenerationHelperDependencies dependencies )
            : base( dependencies )
        {
        }

        public override string DelimitIdentifier( string name, string schema )
        {
            if ( string.IsNullOrEmpty( schema ) )
            {
                return base.DelimitIdentifier( name );
            }

            return base.DelimitIdentifier( schema + "_" + name );
        }

        public override void DelimitIdentifier( StringBuilder builder, string name, string schema )
        {
            if ( string.IsNullOrEmpty( schema ) )
            {
                base.DelimitIdentifier( builder, name );
            }
            else
            {
                base.DelimitIdentifier( builder, schema + "_" + name );
            }
        }
    }
#pragma warning restore EF1001
}
