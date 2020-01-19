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

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// An exception that identifies an error about a call to get an extension
    /// when that extension has not been installed.
    /// </summary>
    public class ExtensionNotFoundException : Exception
    {
        #region Properties

        /// <summary>
        /// The type of extension request that caused the exception.
        /// </summary>
        public Type ExtensionType { get; set; }

        #endregion

        /// <summary>
        /// Creates a new instance of the <see cref="ExtensionNotFoundException"/> class.
        /// </summary>
        /// <param name="extensionType">The type of extension request that caused the exception.</param>
        public ExtensionNotFoundException( Type extensionType )
            : base( "Specified extension was not found." )
        {
            ExtensionType = extensionType;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ExtensionNotFoundException"/> class.
        /// </summary>
        /// <param name="extensionType">The type of extension request that caused the exception.</param>
        /// <param name="message">The exception message.</param>
        public ExtensionNotFoundException( Type extensionType, string message )
            : base( message )
        {
            ExtensionType = extensionType;
        }
    }
}
