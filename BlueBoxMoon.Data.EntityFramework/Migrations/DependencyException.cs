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

using BlueBoxMoon.Data.EntityFramework.Internals;

namespace BlueBoxMoon.Data.EntityFramework.Migrations
{
    /// <summary>
    /// Identifies an exception while trying to 
    /// </summary>
    public class DependencyException : Exception
    {
        #region Properties

        /// <summary>
        /// The name of the plugin that had a dependency exception.
        /// </summary>
        public string Plugin { get; }

        /// <summary>
        /// The version number of the migration that had a dependency exception.
        /// </summary>
        public SemanticVersion Version { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="DependencyException"/> class.
        /// </summary>
        /// <param name="plugin">The name of the plugin.</param>
        /// <param name="version">The version number of the migration.</param>
        /// <param name="step">The specific migration step of the migration.</param>
        /// <param name="message">The message that describes what happened.</param>
        public DependencyException( string plugin, SemanticVersion version, string message )
            : base( message )
        {
            Plugin = plugin;
            Version = version;
        }

        #endregion
    }
}
