using SoulNETLib.Common.Extension;
using SoulNETLibTests.Common.TestData.Models;
using Xunit;

namespace SoulNETLib.Tests.Extension;

public class ListExtensionTests
{
    [Fact]
    public void Replace_CorrectMatch_ReplacesValue()
    {
        // Arrange
        var list = new List<TestObj>
        {
            new() { id = 1, str = SampleStrings.str1 },
            new() { id = 2, str = SampleStrings.str2 },
            new() { id = 3, str = SampleStrings.str3 },
            new() { id = 4, str = SampleStrings.str4 },
            new() { id = 5, str = SampleStrings.str5 },
            new() { id = 6, str = SampleStrings.str6 },
        };

        // Act
        var ret = list.Replace((to) => to.id == 1, new() { id = 1, str = SampleStrings.str7 });

        // Assert
        Assert.Equal(0, ret);
        Assert.Equal(SampleStrings.str7, list[ret].str);
        Assert.Equal(1, list[ret].id);
        Assert.Equal(6, list.Count);
    }

    [Fact]
    public void Replace_IncorrectMatch_ReturnsNegative()
    {
        // Arrange
        var list = new List<TestObj>
        {
            new() { id = 1, str = SampleStrings.str1 },
            new() { id = 2, str = SampleStrings.str2 },
            new() { id = 3, str = SampleStrings.str3 },
            new() { id = 4, str = SampleStrings.str4 },
            new() { id = 5, str = SampleStrings.str5 },
            new() { id = 6, str = SampleStrings.str6 },
        };

        // Act
        var ret = list.Replace((to) => to.id == 69, new() { id = 69, str = SampleStrings.str7 });

        // Assert
        Assert.Equal(-1, ret);
    }

    private sealed class TestObj
    {
        public int id;
        public string? str;
    }
}
