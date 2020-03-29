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
namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// A single error that describes a validation error.
    /// </summary>
    public class ValidationError
    {
        #region Properties

        /// <summary>
        /// The name of the property that was considered invalid. May be
        /// <c>null</c> if a single property was not the cause of the error.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// User friendly message describing the nature of the error.
        /// </summary>
        public string Message { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property that the error is related to.</param>
        /// <param name="message">The message describing the error.</param>
        public ValidationError( string propertyName, string message )
        {
            PropertyName = propertyName;
            Message = message;
        }

        #endregion
    }
}
