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
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

using BlueBoxMoon.Data.EntityFramework.EntityTypes;
using BlueBoxMoon.Linqson;

namespace BlueBoxMoon.Data.EntityFramework.Facets
{
    /// <summary>
    /// Defines an facet that will be recorded in the database.
    /// </summary>
    [Table( "Facets", Schema = FacetsPlugin.Schema )]
    [Guid( "be3d011a-ddd2-4c27-9a48-45583f1ba846" )]
    public class Facet : Entity
    {
        #region Database Properties

        /// <summary>
        /// The entity type identifier for this facet.
        /// </summary>
        public long EntityTypeId
        {
            get => ( long ) GetValue();
            set => SetValue( value );
        }

        /// <summary>
        /// The user friendly name of the facet.
        /// </summary>
        [Required]
        [MaxLength( 100 )]
        public string Name
        {
            get => ( string ) GetValue();
            set => SetValue( value );
        }

        /// <summary>
        /// A short identifier of the facet that can be used by the user
        /// to specify the facet without having to know it's Guid value.
        /// If multiple Key values match on a specific EntityTypeId then the
        /// first match is used.
        /// </summary>
        [MaxLength( 50 )]
        public string Key
        {
            get => ( string ) GetValue();
            set => SetValue( value );
        }

        /// <summary>
        /// The description of this facet that would help the user understand
        /// how to use it.
        /// </summary>
        public string Description
        {
            get => ( string ) GetValue();
            set => SetValue( value );
        }

        /// <summary>
        /// Json encoded data about how this facet should be matched to
        /// entities beyond just the EntityTypeId.
        /// </summary>
        public string Qualifiers
        {
            get => ( string ) GetValue();
            set => SetValue( value );
        }

        /// <summary>
        /// The default value if no value has been set for an entity.
        /// </summary>
        [Required( AllowEmptyStrings = true )]
        public string DefaultValue
        {
            get => ( string ) GetValue();
            set => SetValue( value );
        }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The <see cref="EntityType"/> that this facet belongs to.
        /// </summary>
        public virtual EntityType EntityType { get; set; }

        /// <summary>
        /// All <see cref="FacetValue"/> objects that belong to this
        /// <see cref="Facet"/>.
        /// </summary>
        public virtual ObservableCollection<FacetValue> FacetValues { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the <see cref="Qualifiers"/> property to the encoded predicate
        /// supplied.
        /// </summary>
        /// <typeparam name="TEntity">The entity type this predicate relates to.</typeparam>
        /// <param name="predicate">The predicate that will be used to filter the entities.</param>
        public void SetQualifiers<TEntity>( Expression<Func<TEntity, bool>> predicate )
            where TEntity : IEntity
        {
            var encodedExpression = EncodedExpression.EncodeExpression( predicate );

            // TODO: Serialize and store in this.Qualifiers
        }

        /// <summary>
        /// Gets the qualifiers function to be used for additional filtering.
        /// </summary>
        /// <typeparam name="TEntity">The type to cast the function to.</typeparam>
        /// <returns>A function or <c>null</c>.</returns>
        public Func<TEntity, bool> GetQualifiers<TEntity>()
            where TEntity : IEntity
        {
            return ( Func<TEntity, bool> ) GetQualifiers();
        }

        /// <summary>
        /// Gets the qualifiers generic function to be used for additional filtering.
        /// </summary>
        /// <returns>A function or <c>null</c>.</returns>
        public Delegate GetQualifiers()
        {
            return null;
            // TODO: Deserialize from this.Qualifiers
            //var encodedExpression = ( EncodedExpression ) null;

            //var expression = ( LambdaExpression ) EncodedExpression.DecodeExpression( encodedExpression );

            //return expression.Compile();
        }

        #endregion
    }
}
