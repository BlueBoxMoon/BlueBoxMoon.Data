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
