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
using BlueBoxMoon.Data.EntityFramework.Common.Cache;

namespace BlueBoxMoon.Data.EntityFramework.Common.EntityTypes
{
    /// <summary>
    /// Defines a cached <see cref="EntityType"/> object.
    /// </summary>
    public class CachedEntityType : CachedEntity
    {
        /// <summary>
        /// The full class name of the entity.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The fully qualified class name and assembly information.
        /// </summary>
        public string QualifiedName { get; private set; }

        /// <summary>
        /// Updates the cached information from the entity.
        /// </summary>
        /// <param name="entity">The database entity.</param>
        public override void UpdateFromEntity( IEntity entity )
        {
            base.UpdateFromEntity( entity );

            var entityType = ( EntityType ) entity;

            Name = entityType.Name;
            QualifiedName = entityType.QualifiedName;
        }
    }
}
