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

using BlueBoxMoon.Data.EntityFramework;

using Microsoft.EntityFrameworkCore;

namespace BlueBoxMoon.Data.EntityFramework.Sqlite
{
    /// <summary>
    /// Contains the properties and methods used to interact with a SQLite
    /// database.
    /// </summary>
    internal class SqliteEntityDatabaseProvider : EntityDatabaseProvider
    {
        /// <summary>
        /// Retrieves the required database features provider.
        /// </summary>
        public override IEntityDatabaseFeatures Features { get; } = new SqliteEntityDatabaseFeatures();

        /// <summary>
        /// Called when the EntityDbContext needs to create it's model.
        /// </summary>
        /// <param name="modelBuilder">The instance that handles building the model.</param>
        public override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );

            modelBuilder.UseValueConverterForPropertyType<DateTimeOffset>( new DateTimeOffsetValueConverter() );
            modelBuilder.UseValueConverterForPropertyType<DateTimeOffset?>( new DateTimeOffsetValueConverter() );
        }
    }
}
