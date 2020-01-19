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
using System.Collections.Generic;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Defines the configuration options that will be used by
    /// <see cref="EntityDbContext"/> instances.
    /// </summary>
    public class EntityDbContextOptions : IReadOnlyExtensible
    {
        #region Fields

        private Dictionary<Type, object> _extensions = new Dictionary<Type, object>();

        #endregion

        #region Properties

        /// <summary>
        /// The pre- and post-save hooks that will be used. The order these
        /// hooks are called is not guaranteed.
        /// </summary>
        public IReadOnlyList<EntityDbContextSaveHooks> SaveHooks { get; } = new List<EntityDbContextSaveHooks>();

        /// <summary>
        /// Gets the registered entities.
        /// </summary>
        /// <value>
        /// The registered entities.
        /// </value>
        public IReadOnlyDictionary<Type, EntitySettings> RegisteredEntities { get; } = new Dictionary<Type, EntitySettings>();

        /// <summary>
        /// Gets the registered plugins.
        /// </summary>
        /// <value>
        /// The registered plugins.
        /// </value>
        public IReadOnlyList<EntityPlugin> Plugins { get; } = new List<EntityPlugin>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDbContextOptions"/> class.
        /// </summary>
        internal EntityDbContextOptions()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds the extension for the given type associated with this instance.
        /// </summary>
        /// <typeparam name="TExtension">The type of extension to retrieve.</typeparam>
        /// <returns>An instance of <typeparamref name="TExtension"/> or <c>null</c> if not found.</returns>
        public TExtension FindExtension<TExtension>()
            where TExtension : class
        {
            if ( _extensions.TryGetValue( typeof( TExtension ), out var extension ) )
            {
                return ( TExtension ) extension;
            }

            return null;
        }

        /// <summary>
        /// Gets the extension for the given type associated with this instance. Throws
        /// an exception if extension is not found.
        /// </summary>
        /// <typeparam name="TExtension">The type of extension to retrieve.</typeparam>
        /// <returns>An instance of <typeparamref name="TExtension"/>.</returns>
        public TExtension GetExtension<TExtension>()
            where TExtension : class
        {
            var extension = FindExtension<TExtension>();

            if ( extension == null )
            {
                throw new ExtensionNotFoundException( typeof( TExtension ) );
            }

            return extension;
        }

        /// <summary>
        /// Adds or replaces an extension.
        /// </summary>
        /// <typeparam name="TExtension">The type of extension to be stored.</typeparam>
        /// <param name="extension">The extension instance.</param>
        internal void AddOrReplaceExtension<TExtension>( TExtension extension )
            where TExtension : class
        {
            _extensions[typeof( TExtension )] = extension;
        }

        #endregion
    }
}
