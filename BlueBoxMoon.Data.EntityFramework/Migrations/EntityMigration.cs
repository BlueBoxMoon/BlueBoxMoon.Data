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

namespace BlueBoxMoon.Data.EntityFramework.Migrations
{
    /// <summary>
    /// The base class that all migrations must inherit from.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.Migrations.Migration" />
    public abstract class EntityMigration : Migration
    {
        #region Properties

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        /// <value>
        /// The service provider.
        /// </value>
        internal protected IServiceProvider ServiceProvider { get; internal set; }

        /// <summary>
        /// Gets the database features.
        /// </summary>
        /// <value>
        /// The database features.
        /// </value>
        protected IEntityDatabaseFeatures DatabaseFeatures => ServiceProvider.GetService<IEntityDatabaseFeatures>();

        /// <summary>
        /// <para>
        /// The <see cref="T:Microsoft.EntityFrameworkCore.Migrations.Operations.MigrationOperation" />s that will migrate the database 'up'.
        /// </para>
        /// <para>
        /// That is, those operations that need to be applied to the database
        /// to take it from the state left in by the previous migration so that it is up-to-date
        /// with regard to this migration.
        /// </para>
        /// </summary>
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

        /// <summary>
        /// <para>
        /// The <see cref="T:Microsoft.EntityFrameworkCore.Migrations.Operations.MigrationOperation" />s that will migrate the database 'down'.
        /// </para>
        /// <para>
        /// That is, those operations that need to be applied to the database
        /// to take it from the state left in by this migration so that it returns to the
        /// state that it was in before this migration was applied.
        /// </para>
        /// </summary>
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

        #endregion

        #region Methods

        /// <summary>
        /// Builds the operations.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        private List<MigrationOperation> BuildOperations( Action<MigrationBuilder> action )
        {
            var migrationBuilder = new EntityMigrationBuilder( ActiveProvider, DatabaseFeatures );

            action( migrationBuilder );

            return migrationBuilder.Operations;
        }

        #endregion
    }
}
