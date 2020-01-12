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
using System.Collections.Generic;

using BlueBoxMoon.Data.EntityFramework.Internals;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Defines the configuration options that will be used by
    /// <see cref="ModelDbContext"/> instances.
    /// </summary>
    public class ModelDbContextOptions
    {
        #region Properties

        /// <summary>
        /// The pre- and post-save hooks that will be used. The order these
        /// hooks are called is not guaranteed.
        /// </summary>
        public IReadOnlyList<ModelDbContextSaveHooks> SaveHooks { get; } = new List<ModelDbContextSaveHooks>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelDbContextOptions"/> class.
        /// </summary>
        internal ModelDbContextOptions()
        {
        }

        #endregion
    }
}
