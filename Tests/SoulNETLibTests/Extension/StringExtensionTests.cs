using FluentAssertions;
using SoulNETLib.Common.Extension;
using SoulNETLibTests.Common.TestData.Models;
using System;
using Xunit;

namespace SoulNETLibTests.Extension
{
    public class StringExtensionTests
    {
        #region OrEmptyIfNull

        [Fact]
        public void OrEmptyIfNull_NullString_ReturnEmpty()
        {
            // Arrange
            #pragma warning disable CS8600
            string nullStr = null;
            #pragma warning restore CS8600
            string? nullableNullStr = null;

            // Act
            var ret = nullStr.OrEmptyIfNull();
            var ret2 = nullableNullStr.OrEmptyIfNull();

            // Assert
            ret.Should().BeEmpty();
            ret2.Should().BeEmpty();
        }

        [Fact]
        public void OrEmptyIfNull_InitializedString_ReturnSource()
        {
            // Arrange
            var str = SampleStrings.str1;

            // Act
            var ret = str.OrEmptyIfNull();

            // Assert
            ret.Should().BeSameAs(str);
            ret.Should().BeEquivalentTo(SampleStrings.str1);
        }

        #endregion

        #region RemoveWhitespaces

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("          ")]
        [InlineData(" ")]
        [InlineData(" ")]
        [InlineData(" ")]
        [InlineData("      　 ")]
        public void RemoveWhitespaces_OnlyWhitespace_ReturnEmpty(string whitespaceString)
        {
            // Arrange

            // Act
            var ret = whitespaceString.RemoveWhitespaces();

            // Assert
            ret.Should().Be(string.Empty);
        }

        [Fact]
        public void RemoveWhitespaces_WhitespacesOnSides_ReturnString()
        {
            // Arrange
            string whitespaceString = "      string  　 ";

            // Act
            var ret = whitespaceString.RemoveWhitespaces();

            // Assert
            ret.Should().Be("string");
        }

        [Fact]
        public void RemoveWhitespaces_WhitespacesInMiddle_ReturnString()
        {
            // Arrange
            string whitespaceString = "st      ri  　 ng";

            // Act
            var ret = whitespaceString.RemoveWhitespaces();

            // Assert
            ret.Should().Be("string");
        }

        [Fact]
        public void RemoveWhitespaces_WithNull_ReturnNull()
        {
            // Arrange
            #pragma warning disable CS8600 // Testing null value to non-nullable type.
            string nullString = null;
            #pragma warning restore CS8600

            // Act
            #pragma warning disable CS8604 // Testing null reference argument.
            Func<string> act = () => nullString.RemoveWhitespaces();
            #pragma warning restore CS8604

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        #endregion

        #region TryRemoveEnd

        [Fact]
        public void TryRemoveEnd_WithMatchingEnd_ReturnTrueRemoveEnd()
        {
            // Arrange
            string testString = "Lorem ipsum dolor sit amet";

            // Act
            var removed = testString.TryRemoveEnd(" amet", out string parsed);

            // Assert
            removed.Should().BeTrue();
            parsed.Should().Be("Lorem ipsum dolor sit");
        }

        [Fact]
        public void TryRemoveEnd_WithIncorrectEnd_ReturnFalseKeepEnd()
        {
            // Arrange
            string testString = "Lorem ipsum dolor sit amet";

            // Act
            var removed = testString.TryRemoveEnd(" aset", out string parsed);

            // Assert
            removed.Should().BeFalse();
            parsed.Should().Be("Lorem ipsum dolor sit amet");
        }

        [Fact]
        public void TryRemoveEnd_WithNullSource_ThrowsArgumentNullException()
        {
            // Arrange
            #pragma warning disable CS8600 // Testing null value to non-nullable type.
            string nullString = null;
            #pragma warning restore CS8600

            // Act
            #pragma warning disable CS8604 // Testing null reference argument.
            Func<bool> act = () => nullString.TryRemoveEnd(" amet", out string parsed);
            #pragma warning restore CS8604

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void TryRemoveEnd_WithNullEndStr_ThrowsArgumentNullException()
        {
            // Arrange
            string nullString = SampleStrings.str1;

            // Act
            #pragma warning disable CS8625 // Testing null literal to non-nullable reference type.
            Func<bool> act = () => nullString.TryRemoveEnd(null, out string parsed);
            #pragma warning restore CS8625

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        #endregion

        #region TryRemoveStart

        [Fact]
        public void TryRemoveStart_WithMatchingStart_ReturnTrueRemoveStart()
        {
            // Arrange
            string testString = "Lorem ipsum dolor sit amet";

            // Act
            var removed = testString.TryRemoveStart("Lorem ", out string parsed);

            // Assert
            removed.Should().BeTrue();
            parsed.Should().Be("ipsum dolor sit amet");
        }

        [Fact]
        public void TryRemoveStart_WithIncorrectStart_ReturnFalseKeepStart()
        {
            // Arrange
            string testString = "Lorem ipsum dolor sit amet";

            // Act
            var removed = testString.TryRemoveStart("Lodem ", out string parsed);

            // Assert
            removed.Should().BeFalse();
            parsed.Should().Be("Lorem ipsum dolor sit amet");
        }

        [Fact]
        public void TryRemoveStart_WithNullSource_ThrowsArgumentNullException()
        {
            // Arrange
            #pragma warning disable CS8600 // Testing null value to non-nullable type.
            string nullString = null;
            #pragma warning restore CS8600

            // Act
            #pragma warning disable CS8604 // Testing null reference argument.
            Func<bool> act = () => nullString.TryRemoveStart("Lorem ", out string parsed);
            #pragma warning restore CS8604

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void TryRemoveStart_WithNullEndStr_ThrowsArgumentNullException()
        {
            // Arrange
            string nullString = SampleStrings.str1;

            // Act
            #pragma warning disable CS8625 // Testing null literal to non-nullable reference type.
            Func<bool> act = () => nullString.TryRemoveStart(null, out string parsed);
            #pragma warning restore CS8625

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        #endregion

    }
}
