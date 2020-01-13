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

using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueBoxMoon.Data.EntityFramework.Migrations
{
    /// <summary>
    /// Provides functionality to access a repository of information
    /// about the migrations that have been executed related to
    /// a specific plugin.
    /// </summary>
    public interface IPluginHistoryRepository
    {
        /// <summary>
        /// Checks for the existence of the repository.
        /// </summary>
        /// <returns><c>true</c> if the repository exists.</returns>
        bool Exists();

        /// <summary>
        /// Gets the SQL script to be executed in order to create
        /// the repository.
        /// </summary>
        /// <returns>A string containing the SQL statement.</returns>
        string GetCreateScript();

        /// <summary>
        /// Gets the applied migrations for the specified plugin.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <returns>A read only list of <see cref="HistoryRow"/> objects that represent which migrations have been run.</returns>
        IReadOnlyList<HistoryRow> GetAppliedMigrations( EntityPlugin plugin );

        /// <summary>
        /// Gets the SQL script to be executed in order to save the <see cref="HistoryRow"/>
        /// to the database.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="historyRow">The history row.</param>
        /// <returns>A string containg the SQL statement.</returns>
        string GetInsertScript( EntityPlugin plugin, HistoryRow historyRow );

        /// <summary>
        /// Gets the SQL script to be executed in order to delete the
        /// specified migration's <see cref="HistoryRow"/> from the database.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="migrationId">The migration identifier.</param>
        /// <returns>A string containing the SQL statement.</returns>
        string GetDeleteScript( EntityPlugin plugin, string migrationId );
    }
}
