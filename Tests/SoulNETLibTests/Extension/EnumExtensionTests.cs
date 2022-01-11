using FluentAssertions;
using SoulNETLib.Extension;
using SoulNETLibTests.Common.TestData.Enums;
using SoulNETLibTests.Common.TestData.Models;
using System;
using Xunit;

namespace SoulNETLibTests.Extension
{
    public class EnumExtensionTests
    {

        #region GetDescription

        [Fact]
        public void GetDescription_HasDescription_ReturnDescription()
        {
            // Arrange
            var sample = SampleEnum.One;

            // Act
            var ret = sample.GetDescription();

            // Assert
            ret.Should().Be(SampleStrings.str1);
        }

        [Fact]
        public void GetDescription_NoDescription_ReturnToString()
        {
            // Arrange
            var sample = SampleEnum.Ten;

            // Act
            var ret = sample.GetDescription();

            // Assert
            ret.Should().Be(sample.ToString());
        }

        [Fact]
        public void GetDescription_NoMatchingValue_ReturnToString()
        {
            // Arrange
            var evil = 666;
            SampleEnum sample = (SampleEnum)evil;

            // Act
            var ret = sample.GetDescription();

            // Assert
            ret.Should().Be(evil.ToString());
        }

        [Fact]
        public void GetDescription_DefaultEnum_ReturnZeroToString()
        {
            // Arrange
            SampleEnum sample = default;

            // Act
            var ret = sample.GetDescription();

            // Assert
            ret.Should().Be(0.ToString());
        }

        #endregion

    }
}
