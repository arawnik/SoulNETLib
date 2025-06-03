using SoulNETLib.Common.Extension;
using SoulNETLibTests.Common.TestData.Models;
using Xunit;

namespace SoulNETLib.Tests.Extension;

public class IEnumerableExtensionTests
{
    #region OrEmptyIfNull

    [Theory]
    [InlineData(typeof(List<string>))]
    [InlineData(typeof(Queue<string>))]
    [InlineData(typeof(Stack<string>))]
    [InlineData(typeof(LinkedList<string>))]
    public void OrEmptyIfNull_NullSources_ReturnEmpty(Type type)
    {
        // Arrange
        var enumerable = Activator.CreateInstance(type) as IEnumerable<string>;

        // Act
        var ret = enumerable.OrEmptyIfNull();

        // Assert
        Assert.Empty(ret);
    }

    [Fact]
    public void OrEmptyIfNull_InitializedList_ReturnSource()
    {
        // Arrange
        var enumerable = new List<string> { SampleStrings.str1, SampleStrings.str2 };

        // Act
        var ret = enumerable.OrEmptyIfNull();

        // Assert
        Assert.Same(enumerable, ret);
        Assert.Equal(enumerable, ret);
    }

    [Fact]
    public void OrEmptyIfNull_InitializedArray_ReturnSource()
    {
        // Arrange
        var enumerable = new string[] { SampleStrings.str1, SampleStrings.str2 };

        // Act
        var ret = enumerable.OrEmptyIfNull();

        // Assert
        Assert.Same(enumerable, ret);
        Assert.Equal(enumerable, ret);
    }

    #endregion

    #region WhereIf

    [Fact]
    public void WhereIf_WithTrue_ApplyFilter()
    {
        // Arrange
        var list = SampleStrings.GetAsList();
        var listWithStr1 = new List<string> { SampleStrings.str1 };

        // Act
        var ret = list.WhereIf(true, x => x.Equals(SampleStrings.str1, StringComparison.Ordinal));

        // Assert
        Assert.Single(ret);
        Assert.Equal(listWithStr1, ret);
        Assert.NotSame(listWithStr1, ret);
        Assert.NotSame(list, ret);
    }

    [Fact]
    public void WhereIf_WithFalse_SkipFilter()
    {
        // Arrange
        var list = SampleStrings.GetAsList();
        var originalListCount = list.Count;
        var listWithStr1 = new List<string> { SampleStrings.str1 };

        // Act
        var ret = list.WhereIf(false, x => x.Equals(SampleStrings.str1, StringComparison.Ordinal));

        // Assert
        Assert.Equal(originalListCount, ret.Count());
        Assert.NotEqual(listWithStr1, ret);
        Assert.NotSame(listWithStr1, ret);
        Assert.Equal(list, ret);
        Assert.Same(list, ret);
    }

    #endregion
}
