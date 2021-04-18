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
    /// Defines a single migration for a plugin by its version number
    /// and step it should be executed in.
    /// </summary>
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
    public class PluginMigrationAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// The version number in major.minor[.build[.revision]] format. This
        /// is the version number that this migration was introduced in.
        /// </summary>
        public SemanticVersion Version { get; }

        /// <summary>
        /// The migration identifier to be written to the database.
        /// </summary>
        internal string MigrationId => Version.ToString();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginMigrationAttribute"/> class
        /// with a default <see cref="Step"/> value of 1.
        /// </summary>
        /// <param name="version">The version number this migration was introduced in.</param>
        public PluginMigrationAttribute( string version )
            : this( version, 1 )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginMigrationAttribute"/> class.
        /// </summary>
        /// <param name="version">The version number this migration was introduced in.</param>
        /// <param name="step">The ordered step to execute this migration in relation to the <paramref name="version"/>.</param>
        public PluginMigrationAttribute( string version, int step )
        {
            if ( version.Contains( "-" ) )
            {
                throw new ArgumentException( "Version must be in format \"<major>[.<minor>[.<patch>]]\".", nameof( version ) );
            }

            Version = SemanticVersion.Parse( version + $"-{step}" );
        }

        #endregion
    }
}
