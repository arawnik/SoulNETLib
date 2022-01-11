using FluentAssertions;
using SoulNETLib.Common.Extension;
using SoulNETLibTests.Common.TestData.Models;
using System;
using Xunit;

namespace SoulNETLibTests.Extension
{
    public class ExceptionExtensionTests
    {

        #region GetDetailMessage

        [Fact]
        public void GetDetailMessage_BothExceptions_ReturnInnerMessage()
        {
            // Arrange
            var exception = new Exception(SampleStrings.str2, new Exception(SampleStrings.str1));

            // Act
            var ret = exception.GetDetailMessage();

            // Assert
            ret.Should().Be(SampleStrings.str1);
        }

        [Fact]
        public void GetDetailMessage_NoInnerException_ReturnMessage()
        {
            // Arrange
            var exception = new Exception(SampleStrings.str2);

            // Act
            var ret = exception.GetDetailMessage();

            // Assert
            ret.Should().Be(SampleStrings.str2);
        }

        [Fact]
        public void GetDetailMessage_NullInnerException_ReturnMessage()
        {
            // Arrange
            var exception = new Exception(SampleStrings.str2, null);

            // Act
            var ret = exception.GetDetailMessage();

            // Assert
            ret.Should().Be(SampleStrings.str2);
        }

        #endregion

    }
}
