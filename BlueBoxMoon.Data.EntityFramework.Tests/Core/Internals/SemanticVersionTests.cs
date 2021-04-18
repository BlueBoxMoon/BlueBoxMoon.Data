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

using BlueBoxMoon.Data.EntityFramework.Internals;

using NUnit.Framework;

namespace BlueBoxMoon.Data.EntityFramework.Tests.Core.Internals
{
    public class SemanticVersionTests
    {
        [Test]
        public void CanParseMajorVersionNumber()
        {
            var success = SemanticVersion.TryParse( "10", out var version );

            Assert.IsTrue( success );

            Assert.AreEqual( 10, version.Major );
            Assert.AreEqual( 0, version.Minor );
            Assert.AreEqual( 0, version.Patch );
            Assert.IsNull( version.Prerelease );
        }

        [Test]
        public void CanParseMajorMinorVersionNumber()
        {
            var success = SemanticVersion.TryParse( "10.4", out var version );

            Assert.IsTrue( success );

            Assert.AreEqual( 10, version.Major );
            Assert.AreEqual( 4, version.Minor );
            Assert.AreEqual( 0, version.Patch );
            Assert.IsNull( version.Prerelease );
        }

        [Test]
        public void CanParseMajorMinorPatchVersionNumber()
        {
            var success = SemanticVersion.TryParse( "10.4.382", out var version );

            Assert.IsTrue( success );

            Assert.AreEqual( 10, version.Major );
            Assert.AreEqual( 4, version.Minor );
            Assert.AreEqual( 382, version.Patch );
            Assert.IsNull( version.Prerelease );
        }

        [Test]
        public void CanParseMajorMinorPatchPrereleaseVersionNumber()
        {
            var success = SemanticVersion.TryParse( "10.4.382-alpha.34", out var version );

            Assert.IsTrue( success );

            Assert.AreEqual( 10, version.Major );
            Assert.AreEqual( 4, version.Minor );
            Assert.AreEqual( 382, version.Patch );
            Assert.AreEqual( "alpha.34", version.Prerelease );
        }

        [Test]
        public void CantParseNullVersionNumber()
        {
            var success = SemanticVersion.TryParse( null, out var version );

            Assert.IsFalse( success );
        }

        [Test]
        public void CantParseEmptyVersionNumber()
        {
            var success = SemanticVersion.TryParse( string.Empty, out var version );

            Assert.IsFalse( success );
        }

        [Test]
        public void CantParseInvalidMajorVersionNumber()
        {
            var success = SemanticVersion.TryParse( "23k.0.0", out _ );

            Assert.IsFalse( success );
        }

        [Test]
        public void CantParseInvalidMinorVersionNumber()
        {
            var success = SemanticVersion.TryParse( "23.4k.0", out _ );

            Assert.IsFalse( success );
        }

        [Test]
        public void CantParseInvalidPatchVersionNumber()
        {
            var success = SemanticVersion.TryParse( "23.4.82alpha", out _ );

            Assert.IsFalse( success );
        }

        [Test]
        public void CantParseTooManySegments()
        {
            var success = SemanticVersion.TryParse( "23.4.82.0", out _ );

            Assert.IsFalse( success );
        }

        [Test]
        public void DoesParseFailureThrowException()
        {
            Assert.Throws<ArgumentException>( () => SemanticVersion.Parse( "bad string" ) );
        }

        [Test]
        public void CanParseValidString()
        {
            var version = SemanticVersion.Parse( "10.4.382-alpha.34" );

            Assert.AreEqual( 10, version.Major );
            Assert.AreEqual( 4, version.Minor );
            Assert.AreEqual( 382, version.Patch );
            Assert.AreEqual( "alpha.34", version.Prerelease );
        }

        [Test]
        public void CanCreateParseMajorVersionNumber()
        {
            var version = new SemanticVersion( 10 );

            Assert.AreEqual( 10, version.Major );
            Assert.AreEqual( 0, version.Minor );
            Assert.AreEqual( 0, version.Patch );
            Assert.IsNull( version.Prerelease );
        }

        [Test]
        public void CanCreateParseMajorMinorVersionNumber()
        {
            var version = new SemanticVersion( 10, 4 );

            Assert.AreEqual( 10, version.Major );
            Assert.AreEqual( 4, version.Minor );
            Assert.AreEqual( 0, version.Patch );
            Assert.IsNull( version.Prerelease );
        }

        [Test]
        public void CanCreateParseMajorMinorPatchVersionNumber()
        {
            var version = new SemanticVersion( 10, 4, 382 );

            Assert.AreEqual( 10, version.Major );
            Assert.AreEqual( 4, version.Minor );
            Assert.AreEqual( 382, version.Patch );
            Assert.IsNull( version.Prerelease );
        }

        [Test]
        public void CanCreateParseMajorMinorPatchPrereleaseVersionNumber()
        {
            var version = new SemanticVersion( 10, 4, 382, "alpha.34" );

            Assert.AreEqual( 10, version.Major );
            Assert.AreEqual( 4, version.Minor );
            Assert.AreEqual( 382, version.Patch );
            Assert.AreEqual( "alpha.34", version.Prerelease );
        }

        [Test]
        public void CantCreateNegativeMajorVersionNumber()
        {
            Assert.Throws<ArgumentOutOfRangeException>( () => new SemanticVersion( -1 ) );
        }

        [Test]
        public void CantCreateNegativeMinorVersionNumber()
        {
            Assert.Throws<ArgumentOutOfRangeException>( () => new SemanticVersion( 10, -1 ) );
        }

        [Test]
        public void CantCreateNegativePatchVersionNumber()
        {
            Assert.Throws<ArgumentOutOfRangeException>( () => new SemanticVersion( 10, 4, -1 ) );
        }

        /// <remarks>
        /// This tests the <see cref="SemanticVersion.GetHashCode"/> method.
        /// </remarks>
        [Test]
        public void CanUseAsDictionaryKey()
        {
            var dictionary = new Dictionary<SemanticVersion, string>();
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 10, 4, 382 );
            var expectedValue = "expected value";

            dictionary.Add( v1, expectedValue );

            var hasValue = dictionary.TryGetValue( v2, out var actualValue );

            Assert.IsTrue( hasValue );
            Assert.AreEqual( expectedValue, actualValue );
        }

        [Test]
        public void IsEqualityCheckTrue()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 10, 4, 382 );

            Assert.IsTrue( v1.Equals( v2 ) );
        }

        [Test]
        public void IsEqualityCheckWithWrongObjectFalse()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );

            Assert.IsFalse( v1.Equals( "10.4.382" ) );
        }

        [Test]
        public void DoesCompareToReturnEqualsForVersionNumberAsObject()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            object v2 = new SemanticVersion( 10, 4, 382 );

            Assert.AreEqual( 0, v1.CompareTo( v2 ) );
        }

        public void DoesCompareToReturnEqualsForVersionNumber()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 10, 4, 382 );

            Assert.AreEqual( 0, v1.CompareTo( v2 ) );
        }

        [Test]
        public void DoesCompareToReturnDescendingOrderForNull()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );

            Assert.AreEqual( 1, v1.CompareTo( null ) );
        }

        [Test]
        public void DoesCompareToPrecedeForMajor()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 8, 4, 382 );

            var actualValue = v1.CompareTo( v2 );

            Assert.Greater( actualValue, 0 );
        }

        [Test]
        public void DoesCompareToFollowForMajor()
        {
            var v1 = new SemanticVersion( 8, 4, 382 );
            var v2 = new SemanticVersion( 10, 4, 382 );

            var actualValue = v1.CompareTo( v2 );

            Assert.Less( actualValue, 0 );
        }

        [Test]
        public void DoesCompareToPrecedeForMinor()
        {
            var v1 = new SemanticVersion( 10, 14, 382 );
            var v2 = new SemanticVersion( 10, 4, 382 );

            var actualValue = v1.CompareTo( v2 );

            Assert.Greater( actualValue, 0 );
        }

        [Test]
        public void DoesCompareToFollowForMinor()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 10, 14, 382 );

            var actualValue = v1.CompareTo( v2 );

            Assert.Less( actualValue, 0 );
        }

        [Test]
        public void DoesCompareToPrecedeForPatch()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 10, 4, 38 );

            var actualValue = v1.CompareTo( v2 );

            Assert.Greater( actualValue, 0 );
        }

        [Test]
        public void DoesCompareToFollowForPatch()
        {
            var v1 = new SemanticVersion( 10, 4, 38 );
            var v2 = new SemanticVersion( 10, 4, 382 );

            var actualValue = v1.CompareTo( v2 );

            Assert.Less( actualValue, 0 );
        }

        [Test]
        public void DoesCompareToPrecedeForPrerelease()
        {
            var v1 = new SemanticVersion( 10, 4, 382, "10" );
            var v2 = new SemanticVersion( 10, 4, 382, "1" );

            var actualValue = v1.CompareTo( v2 );

            Assert.Greater( actualValue, 0 );
        }

        [Test]
        public void DoesCompareToFollowForPrerelease()
        {
            var v1 = new SemanticVersion( 10, 4, 382, "1" );
            var v2 = new SemanticVersion( 10, 4, 382, "10" );

            var actualValue = v1.CompareTo( v2 );

            Assert.Less( actualValue, 0 );
        }
        
        [Test]
        public void DoesComparePrereleaseReturnEqualsForBothNull()
        {
            Assert.AreEqual( 0, SemanticVersion.ComparePrerelease( null, null ) );
        }

        [Test]
        public void DoesComparePrereleaseFollowForNullLeft()
        {
            Assert.Greater( SemanticVersion.ComparePrerelease( null, "alpha.10" ), 0 );
        }

        [Test]
        public void DoesComparePrereleasePrecedeForNullRight()
        {
            Assert.Less( SemanticVersion.ComparePrerelease( "alpha.10", null ), 0 );
        }

        [Test]
        public void DoesComparePrereleaseReturnEqualsWhenBothAreIntegers()
        {
            Assert.AreEqual( 0, SemanticVersion.ComparePrerelease( "23", "23" ) );
        }

        [Test]
        public void DoesComparePrereleasePrecedeWhenLeftIsInteger()
        {
            Assert.Less( SemanticVersion.ComparePrerelease( "23", "alpha23" ), 0 );
        }

        [Test]
        public void DoesComparePrereleaseFollowWhenRightIsInteger()
        {
            Assert.Greater( SemanticVersion.ComparePrerelease( "alpha23", "23" ), 0 );
        }

        [Test]
        public void DoesComparePrereleasePrecedeWhenBothAreStrings()
        {
            Assert.Less( SemanticVersion.ComparePrerelease( "alpha", "beta" ), 0 );
        }

        [Test]
        public void DoesComparePrereleasePrecedeWithFewerComponents()
        {
            Assert.Less( SemanticVersion.ComparePrerelease( "alpha.4", "alpha.4.2" ), 0 );
        }

        [Test]
        public void DoesComparePrereleaseFollowWithMoreComponents()
        {
            Assert.Greater( SemanticVersion.ComparePrerelease( "alpha.4.2", "alpha.4" ), 0 );
        }

        [Test]
        public void IsEqualsOperatorTrueWhenVersionNumbersAreEqual()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 10, 4, 382 );

            Assert.True( v1 == v2 );
        }

        [Test]
        public void IsEqualsOperatorFalseWhenVersionNumbersAreNotEqual()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 12, 4, 382 );

            Assert.False( v1 == v2 );
        }

        [Test]
        public void IsNotEqualsOperatorTrueWhenVersionNumbersAreNotEqual()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 12, 4, 382 );

            Assert.True( v1 != v2 );
        }

        [Test]
        public void IsNotEqualsOperatorFalseWhenVersionNumbersAreEqual()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 10, 4, 382 );

            Assert.False( v1 != v2 );
        }

        [Test]
        public void IsGreaterOperatorFalseWhenLeftIsNull()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );

            Assert.False( null > v1 );
        }

        [Test]
        public void IsGreaterOperatorTrueWhenRightIsNull()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );

            Assert.True( v1 > null );
        }

        [Test]
        public void IsGreaterOperatorTrueWhenLeftGreaterThanRight()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 8, 4, 382 );

            Assert.True( v1 > v2 );
        }

        [Test]
        public void IsGreaterOperatorFalseWhenLeftEqualToRight()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 10, 4, 382 );

            Assert.False( v1 > v2 );
        }

        [Test]
        public void IsGreaterOperatorFalseWhenLeftLessThanRight()
        {
            var v1 = new SemanticVersion( 8, 4, 382 );
            var v2 = new SemanticVersion( 10, 4, 382 );

            Assert.False( v1 > v2 );
        }

        [Test]
        public void IsLessOperatorFalseWhenLeftIsNull()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );

            Assert.False( null < v1 );
        }

        [Test]
        public void IsLessOperatorFalseWhenRightIsNull()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );

            Assert.False( v1 < null );
        }

        [Test]
        public void IsLessOperatorTrueWhenLeftLessThanRight()
        {
            var v1 = new SemanticVersion( 8, 4, 382 );
            var v2 = new SemanticVersion( 10, 4, 382 );

            Assert.True( v1 < v2 );
        }

        [Test]
        public void IsLessOperatorFalseWhenLeftEqualToRight()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 10, 4, 382 );

            Assert.False( v1 < v2 );
        }

        [Test]
        public void IsLessOperatorFalseWhenLeftGreaterThanRight()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 8, 4, 382 );

            Assert.False( v1 < v2 );
        }

        [Test]
        public void IsGreaterOrEqualOperatorFalseWhenLeftIsNull()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );

            Assert.False( null >= v1 );
        }

        [Test]
        public void IsGreaterOrEqualOperatorTrueWhenRightIsNull()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );

            Assert.True( v1 >= null );
        }

        [Test]
        public void IsGreaterOrEqualOperatorTrueWhenLeftGreaterThanRight()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 8, 4, 382 );

            Assert.True( v1 >= v2 );
        }

        [Test]
        public void IsGreaterOrEqualOperatorTrueWhenLeftEqualToRight()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 10, 4, 382 );

            Assert.True( v1 >= v2 );
        }

        [Test]
        public void IsGreaterOrEqualOperatorFalseWhenLeftLessThanRight()
        {
            var v1 = new SemanticVersion( 8, 4, 382 );
            var v2 = new SemanticVersion( 10, 4, 382 );

            Assert.False( v1 >= v2 );
        }

        [Test]
        public void IsLessOrEqualOperatorFalseWhenLeftIsNull()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );

            Assert.False( null <= v1 );
        }

        [Test]
        public void IsLessOrEqualOperatorFalseWhenRightIsNull()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );

            Assert.False( v1 <= null );
        }

        [Test]
        public void IsLessOrEqualOperatorTrueWhenLeftLessThanRight()
        {
            var v1 = new SemanticVersion( 8, 4, 382 );
            var v2 = new SemanticVersion( 10, 4, 382 );

            Assert.True( v1 <= v2 );
        }

        [Test]
        public void IsLessOrEqualOperatorTrueWhenLeftEqualToRight()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 10, 4, 382 );

            Assert.True( v1 <= v2 );
        }

        [Test]
        public void IsLessOrEqualOperatorFalseWhenLeftGreaterThanRight()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );
            var v2 = new SemanticVersion( 8, 4, 382 );

            Assert.False( v1 <= v2 );
        }

        [Test]
        public void IsToStringCorrectWithoutPrerelease()
        {
            var v1 = new SemanticVersion( 10, 4, 382 );

            Assert.AreEqual( "10.4.382", v1.ToString() );
        }

        [Test]
        public void IsToStringCorrectWithPrerelease()
        {
            var v1 = new SemanticVersion( 10, 4, 382, "alpha.24" );

            Assert.AreEqual( "10.4.382-alpha.24", v1.ToString() );
        }
    }
}
