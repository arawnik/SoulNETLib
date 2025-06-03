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
        var ret = sample.GetDescription();

        // Assert
        Assert.Equal(SampleStrings.str1, ret);
    }

    [Fact]
    public void GetDescription_NoDescription_ReturnToString()
    {
        // Arrange
        var sample = SampleEnum.Ten;

        // Act
        var ret = sample.GetDescription();

        // Assert
        Assert.Equal(sample.ToString(), ret);
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
        Assert.Equal(evil.ToString(), ret);
    }

    [Fact]
    public void GetDescription_DefaultEnum_ReturnZeroToString()
    {
        // Arrange
        SampleEnum sample = default;

        // Act
        var ret = sample.GetDescription();

        // Assert
        Assert.Equal(0.ToString(), ret);
    }

    #endregion
}
