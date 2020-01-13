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
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Defines methods that will be called before and after an entity is saved.
    /// </summary>
    public interface IEntityChanges
    {
        /// <summary>
        /// Called before this entity is saved to the database.
        /// </summary>
        /// <param name="dbContext">The database context owning this instance.</param>
        /// <param name="entry">The Entity Framework entry about this instance.</param>
        void PreSaveChanges( EntityDbContext dbContext, EntityEntry entry );

        /// <summary>
        /// Called after this entity has been saved to the database. If the
        /// PreSaveChanges() was called, then it is guaranteed that this method
        /// will also be called.
        /// </summary>
        /// <param name="dbContext">The database context owning this instance.</param>
        /// <param name="success"><c>true</c> if the save was successful.</param>
        void PostSaveChanges( EntityDbContext dbContext, bool success );
    }
}
