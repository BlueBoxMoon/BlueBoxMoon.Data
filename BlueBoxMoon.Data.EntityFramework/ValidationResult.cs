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
using System.Linq;

namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Contains the result of a validation check.
    /// </summary>
    public class ValidationResult
    {
        #region Properties

        /// <summary>
        /// Determines if the result of the validation was successful.
        /// </summary>
        public bool IsValid => _errors.Count == 0;

        /// <summary>
        /// Any error messages related to the validation.
        /// </summary>
        public IEnumerable<ValidationError> Errors => _errors;

        #endregion

        #region Fields

        /// <summary>
        /// The editable list of error messages.
        /// </summary>
        private readonly List<ValidationError> _errors;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        public ValidationResult()
        {
            _errors = new List<ValidationError>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="error">The error for this validation result.</param>
        public ValidationResult( ValidationError error )
        {
            _errors = new List<ValidationError> { error };
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="errors">The error messages for this validation result.</param>
        public ValidationResult( IEnumerable<ValidationError> errors )
        {
            _errors = errors.ToList();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a single error message to the validation result.
        /// </summary>
        /// <param name="error">The error that describes the failed validation.</param>
        public void AddError( ValidationError error )
        {
            _errors.Add( error );
        }

        /// <summary>
        /// Adds multiple error message to the validation result.
        /// </summary>
        /// <param name="errors">The errors that describes the failed validation.</param>
        public void AddError( IEnumerable<ValidationError> errors )
        {
            _errors.AddRange( errors );
        }

        #endregion
    }
}
