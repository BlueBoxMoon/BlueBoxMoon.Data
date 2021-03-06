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
using System.Collections.Generic;

namespace BlueBoxMoon.Data.EntityFramework.Migrations
{
    /// <summary>
    /// Provides functionality to migrate a plugin from the current
    /// version to another version.
    /// </summary>
    public interface IPluginMigrator
    {
        /// <summary>
        /// Initiates a migration operation to the target migration version.
        /// </summary>
        /// <param name="plugin">The plugin whose migrations should be run.</param>
        /// <param name="targetMigration">The target migration.</param>
        void Migrate( EntityPlugin plugin, string targetMigration = null );

        /// <summary>
        /// Initiates a migration operation of the plugins to the latest versions.
        /// </summary>
        /// <param name="plugin">The plugins whose migrations should be run.</param>
        void Migrate( IEnumerable<EntityPlugin> plugins );
    }
}
