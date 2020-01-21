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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace BlueBoxMoon.Data.EntityFramework.EntityTypes
{
    /// <summary>
    /// Defines an entity that will be recorded in the database.
    /// </summary>
    [Table( "EntityTypes", Schema = EntityTypesPlugin.Schema )]
    [Guid( "bcaa4196-9fda-45c1-88da-7cf920a7a0fb" )]
    public class EntityType : Entity
    {
        /// <summary>
        /// The full class name of the entity.
        /// </summary>
        [Required]
        [MaxLength( 100 )]
        public string Name
        {
            get => ( string ) GetValue();
            set => SetValue( value );
        }

        /// <summary>
        /// The fully qualified class name and assembly information.
        /// </summary>
        [Required]
        [MaxLength( 250 )]
        public string QualifiedName
        {
            get => ( string ) GetValue();
            set => SetValue( value );
        }
    }
}
