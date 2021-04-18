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
    /// Indicates a dependency on another plugins migration before this
    /// migration can run.
    /// </summary>
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
    public class DependsOnPluginAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// The plugin class type the migration depends on.
        /// </summary>
        public Type PluginType { get; }

        /// <summary>
        /// The version number of the target plugin that the migration depends on.
        /// </summary>
        public SemanticVersion Version { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="DependsOnPluginAttribute"/> class.
        /// </summary>
        /// <param name="pluginType">The type of plugin to indicate a dependency on.</param>
        /// <param name="version">The version of the plugin that must first be installed.</param>
        public DependsOnPluginAttribute( Type pluginType, string version )
            : this( pluginType, version, int.MaxValue )
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DependsOnPluginAttribute"/> class.
        /// </summary>
        /// <param name="pluginType">The type of plugin to indicate a dependency on.</param>
        /// <param name="version">The version of the plugin that must first be installed.</param>
        /// <param name="step">The migration step in the version number that must first be installed.</param>
        public DependsOnPluginAttribute( Type pluginType, string version, int step )
        {
            if ( version.Contains( "-" ) )
            {
                throw new ArgumentException( "Version must be in format \"<major>[.<minor>[.<patch>]]\".", nameof( version ) );
            }

            PluginType = pluginType;
            Version = SemanticVersion.Parse( version + $"-{step}" );
        }

        #endregion
    }
}
