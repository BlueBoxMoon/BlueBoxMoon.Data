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

namespace BlueBoxMoon.Data.EntityFramework.Internals
{
    /// <summary>
    /// Handles parsing and comparing version numbers.
    /// </summary>
    /// <seealso cref="System.IComparable{BlueBoxMoon.Data.EntityFramework.Internals.SemanticVersion}" />
    public class SemanticVersion : IComparable, IComparable<SemanticVersion>
    {
        #region Properties

        /// <summary>
        /// An empty version that represents version 0.0.0.
        /// </summary>
        public static readonly SemanticVersion Empty = new SemanticVersion( 0 );

        /// <summary>
        /// Gets the major version number.
        /// </summary>
        /// <value>
        /// The major version number.
        /// </value>
        public int Major { get; }

        /// <summary>
        /// Gets the minor version number.
        /// </summary>
        /// <value>
        /// The minor version number.
        /// </value>
        public int Minor { get; }

        /// <summary>
        /// Gets the patch version number.
        /// </summary>
        /// <value>
        /// The patch version number.
        /// </value>
        public int Patch { get; }

        /// <summary>
        /// Gets the pre-release identifier.
        /// </summary>
        /// <value>
        /// The pre-release identifier.
        /// </value>
        public string Prerelease { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersion"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        public SemanticVersion( int major )
            : this( major, 0 )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersion"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        public SemanticVersion( int major, int minor )
            : this( major, minor, 0 )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersion"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <param name="patch">The patch version number.</param>
        public SemanticVersion( int major, int minor, int patch )
            : this( major, minor, patch, null )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersion"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <param name="patch">The patch version number.</param>
        /// <param name="prerelease">The pre-release identifier.</param>
        public SemanticVersion( int major, int minor, int patch, string prerelease )
        {
            if ( major < 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( major ), "Version number cannot be negative." );
            }

            if ( minor < 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( minor ), "Version number cannot be negative." );
            }

            if ( patch < 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( patch ), "Version number cannot be negative." );
            }

            Major = major;
            Minor = minor;
            Patch = patch;
            Prerelease = prerelease;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Parses the version text into a new <see cref="SemanticVersion"/>
        /// instance.
        /// </summary>
        /// <param name="versionText">The version text to be parsed.</param>
        /// <returns>
        /// A new <see cref="SemanticVersion"/> instance that represents
        /// the version.
        /// </returns>
        public static SemanticVersion Parse( string versionText )
        {
            if ( TryParse( versionText, out var version ) )
            {
                return version;
            }

            throw new ArgumentException( "Version number was not in the correct format.", nameof( versionText ) );
        }

        /// <summary>
        /// Attempts to parse the version text into a <see cref="SemanticVersion"/>
        /// instance.
        /// </summary>
        /// <param name="versionText">The version text to be parsed.</param>
        /// <param name="version">On successful return contains the version number.</param>
        /// <returns><c>true</c> if the version text was parsed; <c>false</c> otherwise.</returns>
        public static bool TryParse( string versionText, out SemanticVersion version )
        {
            version = null;

            if ( string.IsNullOrWhiteSpace( versionText ) )
            {
                return false;
            }

            var prereleaseIndex = versionText.IndexOf( '-' );
            string prerelease = null;

            //
            // Check if we have a pre-release version marker and if so extract
            // the pre-release data.
            //
            if ( prereleaseIndex >= 1 )
            {
                prerelease = versionText.Substring( prereleaseIndex + 1 );
                versionText = versionText.Substring( 0, prereleaseIndex );
            }
            
            var components = versionText.Split( new[] { '.' } );

            //
            // If we have too many components then it's not a valid version
            // number.
            //
            if ( components.Length > 3 )
            {
                return false;
            }

            int minor = 0;
            int patch = 0;

            //
            // Attempt to parse each of the version segment numbers.
            //
            if ( !int.TryParse( components[0], out var major ) )
            {
                return false;
            }

            if ( components.Length >= 2 && !int.TryParse( components[1], out minor ) )
            {
                return false;
            }

            if ( components.Length >= 3 && !int.TryParse( components[2], out patch ) )
            {
                return false;
            }

            version = new SemanticVersion( major, minor, patch, prerelease );

            return true;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals( object obj )
        {
            return obj is SemanticVersion ver &&
                     Major == ver.Major &&
                     Minor == ver.Minor &&
                     Patch == ver.Patch &&
                     ( Prerelease ?? string.Empty ) == ( ver.Prerelease ?? string.Empty );
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return HashCode.Combine( Major, Minor, Patch, Prerelease );
        }

        /// <summary>
        /// Compares this object to another object to determine equality.
        /// </summary>
        /// <param name="other">The other object to compare against.</param>
        /// <returns>
        /// A value of 0 to indicate that both versions are equal,
        /// or a value greater than 0 to indicate that this version number is
        /// higher than the other version number, or a value less than 0 to
        /// indicate that this version number is less than the other version
        /// number.
        /// </returns>
        public int CompareTo( object other )
        {
            return CompareTo( ( SemanticVersion ) other );
        }

        /// <summary>
        /// Compares this version number to the other version number.
        /// </summary>
        /// <param name="other">The other version number.</param>
        /// <returns>
        /// A value of 0 to indicate that both versions are equal,
        /// or a value greater than 0 to indicate that this version number is
        /// higher than the other version number, or a value less than 0 to
        /// indicate that this version number is less than the other version
        /// number.
        /// </returns>
        public int CompareTo( SemanticVersion other )
        {
            int result;

            //
            // If other is null, then our version is higher than theirs.
            //
            if ( other == null )
            {
                return 1;
            }

            //
            // Compare the major version numbers.
            //
            result = Major.CompareTo( other.Major );
            if ( result != 0 )
            {
                return result;
            }

            //
            // Compare the minor version numbers.
            //
            result = Minor.CompareTo( other.Minor );
            if ( result != 0 )
            {
                return result;
            }

            //
            // Compare the patch version numbers.
            //
            result = Patch.CompareTo( other.Patch );
            if ( result != 0 )
            {
                return result;
            }

            return ComparePrerelease( Prerelease, other.Prerelease );
        }

        /// <summary>
        /// Compares the prerelease.
        /// </summary>
        /// <param name="left">The left pre-release identifier.</param>
        /// <param name="right">The right pre-release identifier.</param>
        /// <returns>
        /// A value of 0 to indicate that both prerelease values are equal,
        /// or a value greater than 0 to indicate that <paramref name="left"/> is
        /// higher than <paramref name="right"/>, or a value less than 0 to
        /// indicate that <paramref name="left"/> is less than <paramref name="right"/>.
        /// </returns>
        public static int ComparePrerelease( string left, string right )
        {
            //
            // If both strings are null or empty then they are considered equal.
            //
            if ( string.IsNullOrWhiteSpace( left ) && string.IsNullOrWhiteSpace( right ) )
            {
                return 0;
            }

            //
            // Otherwise, if the left string is null or empty then it is
            // considered a higher version number than the right.
            //
            if ( string.IsNullOrWhiteSpace( left ) )
            {
                return 1;
            }

            //
            // Otherwise, if the right string is null or empty then it is
            // considered a higher version number than the left.
            //
            if ( string.IsNullOrWhiteSpace( right ) )
            {
                return -1;
            }

            //
            // At this point, both sides of the equation are non-null
            // and non-empty.
            //
            var leftComponents = left.Split( '.' );
            var rightComponents = right.Split( '.' );
            var length = Math.Min( leftComponents.Length, rightComponents.Length );

            for ( int i = 0; i < length; i++ )
            {
                int result;
                var leftIsNumeric = int.TryParse( leftComponents[i], out var leftNumeric );
                var rightIsNumeric = int.TryParse( rightComponents[i], out var rightNumeric );

                if ( leftIsNumeric && rightIsNumeric )
                {
                    result = leftNumeric.CompareTo( rightNumeric );
                    if ( result != 0 )
                    {
                        return result;
                    }
                }

                //
                // If the left component is numeric and the right is not, then the
                // left side is a lower version number than the right.
                //
                else if ( leftIsNumeric )
                {
                    return -1;
                }

                //
                // If the right component is numeric and the left is not, then the
                // right side is a lower version number than the right.
                //
                else if ( rightIsNumeric )
                {
                    return 1;
                }

                //
                // Both are non-numerics, compre then as strings.
                //
                else
                {
                    result = leftComponents[i].CompareTo( rightComponents[i] );
                    if ( result != 0 )
                    {
                        return result;
                    }
                }

                //
                // Everything is equal, try the next component.
                //
            }

            //
            // Everything was equal up to this point. If the left side has
            // more components than the right then consider it a higher
            // version number.
            //
            return leftComponents.Length.CompareTo( rightComponents.Length );
        }

        /// <summary>
        /// Compares two version numbers for equality.
        /// </summary>
        /// <param name="left">The left version number.</param>
        /// <param name="right">The right version number.</param>
        /// <returns>
        /// <c>true</c> if both version numbers are equal to each other; <c>false</c> otherwise.
        /// </returns>
        public static bool operator ==( SemanticVersion left, SemanticVersion right )
        {
            return EqualityComparer<SemanticVersion>.Default.Equals( left, right );
        }

        /// <summary>
        /// Compares two version numbers for non-equality.
        /// </summary>
        /// <param name="left">The left version number.</param>
        /// <param name="right">The right version number.</param>
        /// <returns>
        /// <c>true</c> if the version numbers are not equal to each other; <c>false</c> otherwise.
        /// </returns>
        public static bool operator !=( SemanticVersion left, SemanticVersion right )
        {
            return !( left == right );
        }

        /// <summary>
        /// Compares two version numbers to see if the left value is greater
        /// than the right value.
        /// </summary>
        /// <param name="left">The left version number.</param>
        /// <param name="right">The right version number.</param>
        /// <returns>
        /// <c>true</c> if the left value has a higher version number than
        /// the right value; <c>false</c> otherwise.
        /// </returns>
        public static bool operator >( SemanticVersion left, SemanticVersion right )
        {
            if ( left == null )
            {
                return false;
            }

            return left.CompareTo( right ) > 0;
        }

        /// <summary>
        /// Compares two version numbers to see if the left value is less
        /// than the right value.
        /// </summary>
        /// <param name="left">The left version number.</param>
        /// <param name="right">The right version number.</param>
        /// <returns>
        /// <c>true</c> if the left value has a lower version number than
        /// the right value; <c>false</c> otherwise.
        /// </returns>
        public static bool operator <( SemanticVersion left, SemanticVersion right )
        {
            if ( left == null )
            {
                return false;
            }

            return left.CompareTo( right ) < 0;
        }

        /// <summary>
        /// Compares two version numbers to see if the left value is greater
        /// than or equal to the right value.
        /// </summary>
        /// <param name="left">The left version number.</param>
        /// <param name="right">The right version number.</param>
        /// <returns>
        /// <c>true</c> if the left value has a higher version number than
        /// the right value or if the two version numbers are equal;
        /// <c>false</c> otherwise.
        /// </returns>
        public static bool operator >=( SemanticVersion left, SemanticVersion right )
        {
            if ( left == null )
            {
                return false;
            }

            return left.CompareTo( right ) >= 0;
        }

        /// <summary>
        /// Compares two version numbers to see if the left value is less
        /// than or equal to the right value.
        /// </summary>
        /// <param name="left">The left version number.</param>
        /// <param name="right">The right version number.</param>
        /// <returns>
        /// <c>true</c> if the left value has a lower version number than
        /// the right value or if the two version numbers are equal;
        /// <c>false</c> otherwise.
        /// </returns>
        public static bool operator <=( SemanticVersion left, SemanticVersion right )
        {
            if ( left == null )
            {
                return false;
            }

            return left.CompareTo( right ) <= 0;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if ( string.IsNullOrWhiteSpace( Prerelease ) )
            {
                return $"{Major}.{Minor}.{Patch}";
            }
            else
            {
                return $"{Major}.{Minor}.{Patch}-{Prerelease}";
            }
        }

        #endregion
    }
}
