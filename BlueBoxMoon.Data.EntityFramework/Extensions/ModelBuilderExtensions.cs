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
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Extension methods for the <see cref="ModelBuilder"/> class.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Use the specified convert for all properties of the given type.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="converter">The converter to use.</param>
        /// <returns>The <see cref="ModelBuilder"/> object.</returns>
		public static ModelBuilder UseValueConverterForPropertyType<T>( this ModelBuilder modelBuilder, ValueConverter converter )
		{
			return modelBuilder.UseValueConverterForPropertyType( typeof( T ), converter );
		}

        /// <summary>
        /// Use the specified convert for all properties of the given type.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="propertyType">The property type.</param>
        /// <param name="converter">The converter to use.</param>
        /// <returns>The <see cref="ModelBuilder"/> object.</returns>
		public static ModelBuilder UseValueConverterForPropertyType( this ModelBuilder modelBuilder, Type propertyType, ValueConverter converter )
		{
			foreach ( var entityType in modelBuilder.Model.GetEntityTypes() )
			{
                //
				// Note that entityType.GetProperties() will throw an exception, so we have to use reflection.
                //
				var properties = entityType.ClrType.GetProperties().Where( p => p.PropertyType == propertyType );

				foreach ( var property in properties )
				{
					modelBuilder.Entity( entityType.Name ).Property( property.Name )
						.HasConversion( converter );
				}
			}

			return modelBuilder;
		}
	}
}
