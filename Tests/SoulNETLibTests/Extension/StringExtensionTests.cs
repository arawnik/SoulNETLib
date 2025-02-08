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
            Assert.Equal(string.Empty, ret);
            Assert.Equal(string.Empty, ret2);
        }

        [Fact]
        public void OrEmptyIfNull_InitializedString_ReturnSource()
        {
            // Arrange
            var str = SampleStrings.str1;

            // Act
            var ret = str.OrEmptyIfNull();

            // Assert
            Assert.Same(str, ret);
            Assert.Equal(SampleStrings.str1, ret);
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
            Assert.Equal(string.Empty, ret);
        }

        [Fact]
        public void RemoveWhitespaces_WhitespacesOnSides_ReturnString()
        {
            // Arrange
            string whitespaceString = "      string  　 ";

            // Act
            var ret = whitespaceString.RemoveWhitespaces();

            // Assert
            Assert.Equal("string", ret);
        }

        [Fact]
        public void RemoveWhitespaces_WhitespacesInMiddle_ReturnString()
        {
            // Arrange
            string whitespaceString = "st      ri  　 ng";

            // Act
            var ret = whitespaceString.RemoveWhitespaces();

            // Assert
            Assert.Equal("string", ret);
        }

        [Fact]
        public void RemoveWhitespaces_WithNull_ReturnNull()
        {
            // Arrange
            string nullString = null!;

            // Act
            Func<string> act = () => nullString.RemoveWhitespaces();

            // Assert
            Assert.Throws<ArgumentNullException>(act);
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
            Assert.True(removed);
            Assert.Equal("Lorem ipsum dolor sit", parsed);
        }

        [Fact]
        public void TryRemoveEnd_WithIncorrectEnd_ReturnFalseKeepEnd()
        {
            // Arrange
            string testString = "Lorem ipsum dolor sit amet";

            // Act
            var removed = testString.TryRemoveEnd(" aset", out string parsed);

            // Assert
            Assert.False(removed);
            Assert.Equal("Lorem ipsum dolor sit amet", parsed);
        }

        [Fact]
        public void TryRemoveEnd_WithNullSource_ThrowsArgumentNullException()
        {
            // Arrange
            string nullString = null!;

            // Act
            Action act = () => nullString.TryRemoveEnd(" amet", out string parsed);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void TryRemoveEnd_WithNullEndStr_ThrowsArgumentNullException()
        {
            // Arrange
            string nullString = SampleStrings.str1;

            // Act
            Action act = () => nullString.TryRemoveEnd(null!, out string parsed);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
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
            Assert.True(removed);
            Assert.Equal("ipsum dolor sit amet", parsed);
        }

        [Fact]
        public void TryRemoveStart_WithIncorrectStart_ReturnFalseKeepStart()
        {
            // Arrange
            string testString = "Lorem ipsum dolor sit amet";

            // Act
            var removed = testString.TryRemoveStart("Lodem ", out string parsed);

            // Assert
            Assert.False(removed);
            Assert.Equal("Lorem ipsum dolor sit amet", parsed);
        }

        [Fact]
        public void TryRemoveStart_WithNullSource_ThrowsArgumentNullException()
        {
            // Arrange
            string nullString = null!;

            // Act
            Action act = () => nullString.TryRemoveStart("Lorem ", out string parsed);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void TryRemoveStart_WithNullEndStr_ThrowsArgumentNullException()
        {
            // Arrange
            string nullString = SampleStrings.str1;

            // Act
            void act() => nullString.TryRemoveStart(null!, out string parsed);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
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
            Assert.Equal("Lorem ipsum dolor sit amet", modified);
        }

        [Fact]
        public void FirstCharToUpper_WithEmptyString_ReturnEmptyString()
        {
            // Arrange
            string testString = string.Empty;

            // Act
            var modified = testString.FirstCharToUpper();

            // Assert
            Assert.Equal(string.Empty, modified);
        }

        [Fact]
        public void FirstCharToUpper_WithNull_ThrowsException()
        {
            // Arrange
            string testString = null!;

            // Act
            Func<string> act = () => testString.FirstCharToUpper();

            // Assert
            Assert.Throws<ArgumentNullException>(act);
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
            Assert.Equal(1234, modified);
        }

        [Fact]
        public void GetDigitsInt_WithEmptyString_ReturnZero()
        {
            // Arrange
            string testString = string.Empty;

            // Act
            var modified = testString.GetDigitsInt();

            // Assert
            Assert.Equal(0, modified);
        }

        [Fact]
        public void GetDigitsInt_WithNull_ThrowsException()
        {
            // Arrange
            string testString = null!;

            // Act
            Action act = () => testString.GetDigitsInt();

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void GetDigitsLong_WithProperString_ReturnParsedInt()
        {
            // Arrange
            string testString = "lorem ipsum dolor 1234 sit amet";

            // Act
            var modified = testString.GetDigitsLong();

            // Assert
            Assert.Equal(1234, modified);
        }

        [Fact]
        public void GetDigitsLong_WithEmptyString_ReturnZero()
        {
            // Arrange
            string testString = string.Empty;

            // Act
            var modified = testString.GetDigitsLong();

            // Assert
            Assert.Equal(0, modified);
        }

        [Fact]
        public void GetDigitsLong_WithNull_ThrowsException()
        {
            // Arrange
            string testString = null!;

            // Act
            Action act = () => testString.GetDigitsLong();

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        #endregion
    }
}
