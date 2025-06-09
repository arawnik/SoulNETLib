using SoulNETLib.Common.Extension;
using SoulNETLibTests.Common.TestData.Enums;
using SoulNETLibTests.Common.TestData.Models;
using Xunit;

namespace SoulNETLib.Tests.Extension;

public class EnumExtensionTests
{
    #region GetDescription

    [Fact]
    public void GetDescription_HasDescription_ReturnDescription()
    {
        // Arrange
        var sample = SampleEnum.One;

        // Act
        var result = sample.GetDescription();

        // Assert
        Assert.Equal(SampleStrings.str1, result);
    }

    [Fact]
    public void GetDescription_NoDescription_ReturnToString()
    {
        // Arrange
        var sample = SampleEnum.Ten;

        // Act
        var result = sample.GetDescription();

        // Assert
        Assert.Equal(sample.ToString(), result);
    }

    [Fact]
    public void GetDescription_NoMatchingValue_ReturnToString()
    {
        // Arrange
        var value = 666;
        SampleEnum sample = (SampleEnum)value;

        // Act
        var result = sample.GetDescription();

        // Assert
        Assert.Equal(value.ToString(), result);
    }

    [Fact]
    public void GetDescription_DefaultEnum_ReturnZeroToString()
    {
        // Arrange
        SampleEnum sample = default;

        // Act
        var result = sample.GetDescription();

        // Assert
        Assert.Equal(0.ToString(), result);
    }

    #endregion

    #region TryParseEnumMember

    [Fact]
    public void TryParseEnumMember_ValidValue_ReturnsTrueAndEnum()
    {
        // Arrange
        var input = "ONE";

        // Act
        var success = input.TryParseEnumMember<SampleEnum>(out var result);

        // Assert
        Assert.True(success);
        Assert.Equal(SampleEnum.One, result);
    }

    [Fact]
    public void TryParseEnumMember_InvalidValue_ReturnsFalse()
    {
        // Arrange
        var input = "InvalidValue";

        // Act
        var success = input.TryParseEnumMember<SampleEnum>(out var result);

        // Assert
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void TryParseEnumMember_CaseInsensitive_ReturnsTrue()
    {
        // Arrange
        var input = "two";

        // Act
        var success = input.TryParseEnumMember<SampleEnum>(out var result);

        // Assert
        Assert.True(success);
        Assert.Equal(SampleEnum.Two, result);
    }

    #endregion

    #region GetEnumMember

    [Fact]
    public void GetEnumMember_HasEnumMember_ReturnsCorrectValue()
    {
        // Arrange
        var value = SampleEnum.One;

        // Act
        var result = value.GetEnumMember();

        // Assert
        Assert.Equal("ONE", result);
    }

    [Fact]
    public void GetEnumMember_NoEnumMember_ReturnsName()
    {
        // Arrange
        var value = SampleEnum.Ten;

        // Act
        var result = value.GetEnumMember();

        // Assert
        Assert.Equal("Ten", result);
    }

    [Fact]
    public void GetEnumMember_InvalidEnumValue_ReturnsValueToString()
    {
        // Arrange
        var value = (SampleEnum)999;

        // Act
        var result = value.GetEnumMember();

        // Assert
        Assert.Equal("999", result);
    }

    #endregion
}
