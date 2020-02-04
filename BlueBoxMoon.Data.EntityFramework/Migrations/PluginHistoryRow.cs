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
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueBoxMoon.Data.EntityFramework.Migrations
{
    /// <summary>
    /// Defines the structure of the Plugin migration tracking table.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.Migrations.HistoryRow" />
    public class PluginHistoryRow : HistoryRow
    {
        #region Properties

        /// <summary>
        /// Gets the plugin identifier.
        /// </summary>
        /// <value>
        /// The plugin identifier.
        /// </value>
        public string Plugin { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginHistoryRow"/> class.
        /// </summary>
        /// <param name="plugin">The plugin identifier.</param>
        /// <param name="migrationId">The migration identifier.</param>
        /// <param name="productVersion">The product version.</param>
        public PluginHistoryRow( string plugin, string migrationId, string productVersion )
            : base( migrationId, productVersion )
        {
            Plugin = plugin;
        }

        #endregion
    }
}
