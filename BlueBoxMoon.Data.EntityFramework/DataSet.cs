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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Provides access to entities in the database.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Linq.IQueryable{T}" />
    /// <seealso cref="System.Collections.Generic.IAsyncEnumerable{T}" />
    public class DataSet<T> : IQueryable<T>, IAsyncEnumerable<T>
        where T : class, IEntity
    {
        #region Properties

        /// <summary>
        /// Gets the context owning this data set.
        /// </summary>
        /// <value>
        /// The context owning this data set.
        /// </value>
        public EntityDbContext Context { get; }

        /// <summary>
        /// Gets the underlying database set.
        /// </summary>
        /// <value>
        /// The underlying database set.
        /// </value>
        public DbSet<T> DbSet { get; }

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable"></see> is executed.
        /// </summary>
        /// <value>
        /// The type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable"></see> is executed.
        /// </value>
        public Type ElementType => ( ( IQueryable ) DbSet ).ElementType;

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable"></see>.
        /// </summary>
        /// <value>
        /// The expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable"></see>.
        /// </value>
        public Expression Expression => ( ( IQueryable ) DbSet ).Expression;

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <value>
        /// The query provider that is associated with this data source.
        /// </value>
        public IQueryProvider Provider => ( ( IQueryable ) DbSet ).Provider;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSet{T}"/> class.
        /// </summary>
        /// <param name="context">The underlying context that will own this data set.</param>
        public DataSet( EntityDbContext context )
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The matched <see cref="T"/> or <c>null</c> if not found.</returns>
        public virtual T GetById( int id )
        {
            return DbSet.FirstOrDefault( a => a.Id == id );
        }

        /// <summary>
        /// Gets the entity by its identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The matched <see cref="T"/> or <c>null</c> if not found.</returns>
        public virtual async Task<T> GetByIdAsync( int id )
        {
            return await DbSet.FirstOrDefaultAsync( a => a.Id == id );
        }

        /// <summary>
        /// Gets the entity by its unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>
        /// The matched <see cref="T" /> or <c>null</c> if not found.
        /// </returns>
        public virtual T GetByGuid( Guid guid )
        {
            return DbSet.FirstOrDefault( a => a.Guid == guid );
        }

        /// <summary>
        /// Gets the entity by its unique identifier asynchronous.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>The matched <see cref="T"/> or <c>null</c> if not found.</returns>
        public virtual async Task<T> GetByGuidAsync( Guid guid )
        {
            return await DbSet.FirstOrDefaultAsync( a => a.Guid == guid );
        }

        /// <summary>
        /// Returns this object as <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> AsQueryable()
        {
            return DbSet.AsQueryable();
        }

        /// <summary>
        /// Adds the specified item to the data set.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Add( T entity )
        {
            DbSet.Add( entity );
        }

        /// <summary>
        /// Adds the range of entities to the data set.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void AddRange( IEnumerable<T> entities )
        {
            DbSet.AddRange( entities );
        }

        /// <summary>
        /// Removes the specified entity from the data set.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Remove( T entity )
        {
            DbSet.Remove( entity );
        }

        /// <summary>
        /// Removes the range of entities from the data set.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void RemoveRange( IEnumerable<T> entities )
        {
            DbSet.RemoveRange( entities );
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return ( ( IEnumerable<T> ) DbSet ).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ( ( IEnumerable<T> ) DbSet ).GetEnumerator();
        }

        /// <summary>
        /// Gets the asynchronous enumerator.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public IAsyncEnumerator<T> GetAsyncEnumerator( CancellationToken cancellationToken = default )
        {
            return ( ( IAsyncEnumerable<T> ) DbSet ).GetAsyncEnumerator( cancellationToken );
        }

        #endregion
    }
}
