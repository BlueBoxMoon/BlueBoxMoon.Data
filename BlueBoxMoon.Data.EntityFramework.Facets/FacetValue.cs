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
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace BlueBoxMoon.Data.EntityFramework.Facets
{
    /// <summary>
    /// Defines an custom facet value that will be recorded in the database.
    /// </summary>
    [Table( "FacetValues", Schema = FacetsPlugin.Schema )]
    [Guid( "bef062b9-9016-46b0-b0f0-6049754e252b" )]
    public class FacetValue : Entity
    {
        #region Database Properties

        /// <summary>
        /// The <see cref="Facet"/> identifier.
        /// </summary>
        public long FacetId
        {
            get => ( long? ) GetValue() ?? 0;
            set => SetValue( value );
        }

        /// <summary>
        /// The entity identifier, in relation to the facet's EntityTypeId.
        /// </summary>
        public long EntityId
        {
            get => ( long? ) GetValue() ?? 0;
            set => SetValue( value );
        }

        /// <summary>
        /// The custom stored value.
        /// </summary>
        public string Value
        {
            get => ( string ) GetValue();
            set => SetValue( value );
        }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The <see cref="Facet"/> that this value belongs to.
        /// </summary>
        public virtual Facet Facet { get; set; }

        #endregion
    }
}
