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
using System.Globalization;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BlueBoxMoon.Data.EntityFramework.Sqlite
{
    /// <summary>
    /// Handles conversion of <see cref="DateTimeOffset"/> values into a format
    /// that can be used in the database.
    /// </summary>
    public class DateTimeOffsetValueConverter : ValueConverter<DateTimeOffset, string>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DateTimeOffsetValueConverter"/> class.
        /// </summary>
        /// <param name="mappingHints"></param>
        public DateTimeOffsetValueConverter( ConverterMappingHints mappingHints = null )
            : base( v => To( v ), v => From( v ), mappingHints )
        {
        }

        /// <summary>
        /// Converts the <see cref="DateTimeOffset"/> value into a format that
        /// SQLite can understand.
        /// </summary>
        /// <param name="value">The runtime value to be converted.</param>
        /// <returns>The value to store in the database.</returns>
        public static string To( DateTimeOffset value )
        {
            return value.ToUniversalTime().ToString( "s" );
        }

        /// <summary>
        /// Converts a database value into one that the runtime can understand.
        /// </summary>
        /// <param name="value">The database value to be converted.</param>
        /// <returns>The runtime value.</returns>
        public static DateTimeOffset From( string value )
        {
            var dateTime = DateTime.ParseExact( value, "s", CultureInfo.InvariantCulture );

            return new DateTimeOffset( DateTime.SpecifyKind( dateTime, DateTimeKind.Utc ) ).ToLocalTime();
        }
    }
}
