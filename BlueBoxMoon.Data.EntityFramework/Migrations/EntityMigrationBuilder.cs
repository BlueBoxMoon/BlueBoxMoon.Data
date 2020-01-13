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

using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueBoxMoon.Data.EntityFramework.Migrations
{
    /// <summary>
    /// A builder providing fluentish API for building entity operations.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.Migrations.MigrationBuilder" />
    public class EntityMigrationBuilder : MigrationBuilder
    {
        #region Properties

        /// <summary>
        /// Gets the database features.
        /// </summary>
        /// <value>
        /// The database features.
        /// </value>
        internal IEntityDatabaseFeatures DatabaseFeatures { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMigrationBuilder"/> class.
        /// </summary>
        /// <param name="activeProvider">The active provider.</param>
        /// <param name="databaseFeatures">The database features.</param>
        public EntityMigrationBuilder( string activeProvider, IEntityDatabaseFeatures databaseFeatures )
            : base( activeProvider )
        {
            DatabaseFeatures = databaseFeatures;
        }

        #endregion
    }
}
