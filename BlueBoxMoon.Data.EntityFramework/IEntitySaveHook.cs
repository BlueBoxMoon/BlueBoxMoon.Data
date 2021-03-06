﻿// MIT License
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
namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Defines an interface that can be implemented by classes wishing to
    /// be used as pre- and post-save hooks for <see cref="EntityDbContext"/> instances.
    /// </summary>
    public interface IEntitySaveHook
    {
        /// <summary>
        /// Called just before changes are saved to the database.
        /// </summary>
        /// <param name="entityDbContext">The database context.</param>
        void PreSaveChanges( EntityDbContext entityDbContext );

        /// <summary>
        /// Called just after changes are saved to the database.
        /// </summary>
        /// <param name="entityDbContext">The database context.</param>
        /// <param name="success"><c>true</c> if the save operation succeeded.</param>
        void PostSaveChanges( EntityDbContext entityDbContext, bool success );
    }
}
