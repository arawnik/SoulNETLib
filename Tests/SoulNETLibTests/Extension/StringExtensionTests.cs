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
            string nullStr = null!;
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
            string nullString = null!;

            // Act
            Func<string> act = () => nullString.RemoveWhitespaces();

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
            string nullString = null!;

            // Act
            Func<bool> act = () => nullString.TryRemoveEnd(" amet", out string parsed);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void TryRemoveEnd_WithNullEndStr_ThrowsArgumentNullException()
        {
            // Arrange
            string nullString = SampleStrings.str1;

            // Act
            Func<bool> act = () => nullString.TryRemoveEnd(null!, out string parsed);

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
            string nullString = null!;

            // Act
            Func<bool> act = () => nullString.TryRemoveStart("Lorem ", out string parsed);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void TryRemoveStart_WithNullEndStr_ThrowsArgumentNullException()
        {
            // Arrange
            string nullString = SampleStrings.str1;

            // Act
            Func<bool> act = () => nullString.TryRemoveStart(null!, out string parsed);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        #endregion

        #region FirstCharToUpper

        [Fact]
        public void FirstCharToUpper_WithProperString_ReturnModifiedString()
        {
            // Arrange
            string testString = "lorem ipsum dolor sit amet";

            // Act
            var modified = testString.FirstCharToUpper();

            // Assert
            modified.Should().Be("Lorem ipsum dolor sit amet");
        }

        [Fact]
        public void FirstCharToUpper_WithEmptyString_ReturnEmptyString()
        {
            // Arrange
            string testString = string.Empty;

            // Act
            var modified = testString.FirstCharToUpper();

            // Assert
            modified.Should().Be(string.Empty);
        }

        [Fact]
        public void FirstCharToUpper_WithNull_ThrowsException()
        {
            // Arrange
            string testString = null!;

            // Act
            Func<string> act = () => testString.FirstCharToUpper();

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        #endregion

        #region GetDigits

        [Fact]
        public void GetDigitsInt_WithProperString_ReturnParsedInt()
        {
            // Arrange
            string testString = "lorem ipsum dolor 1234 sit amet";

            // Act
            var modified = testString.GetDigitsInt();

            // Assert
            modified.Should().Be(1234);
        }

        [Fact]
        public void GetDigitsInt_WithEmptyString_ReturnZero()
        {
            // Arrange
            string testString = string.Empty;

            // Act
            var modified = testString.GetDigitsInt();

            // Assert
            modified.Should().Be(0);
        }

        [Fact]
        public void GetDigitsInt_WithNull_ThrowsException()
        {
            // Arrange
            string testString = null!;

            // Act
            Func<int> act = () => testString.GetDigitsInt();

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void GetDigitsLong_WithProperString_ReturnParsedInt()
        {
            // Arrange
            string testString = "lorem ipsum dolor 1234 sit amet";

            // Act
            var modified = testString.GetDigitsLong();

            // Assert
            modified.Should().Be(1234);
        }

        [Fact]
        public void GetDigitsLong_WithEmptyString_ReturnZero()
        {
            // Arrange
            string testString = string.Empty;

            // Act
            var modified = testString.GetDigitsLong();

            // Assert
            modified.Should().Be(0);
        }

        [Fact]
        public void GetDigitsLong_WithNull_ThrowsException()
        {
            // Arrange
            string testString = null!;

            // Act
            Func<long> act = () => testString.GetDigitsLong();

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        #endregion
    }
}
