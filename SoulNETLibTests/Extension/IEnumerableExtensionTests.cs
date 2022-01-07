using FluentAssertions;
using SoulNETLib.Extension;
using SoulNETLibTests.Common.TestData.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace SoulNETLibTests.Extension
{
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
            ret.Should().BeEmpty();
        }

        [Fact]
        public void OrEmptyIfNull_InitializedList_ReturnSource()
        {
            // Arrange
            var enumerable = new List<string> { SampleStrings.str1, SampleStrings.str2 };

            // Act
            var ret = enumerable.OrEmptyIfNull();

            // Assert
            ret.Should().BeSameAs(enumerable);
            ret.Should().BeEquivalentTo(enumerable);
        }

        [Fact]
        public void OrEmptyIfNull_InitializedArray_ReturnSource()
        {
            // Arrange
            var enumerable = new string[] { SampleStrings.str1, SampleStrings.str2 };

            // Act
            var ret = enumerable.OrEmptyIfNull();

            // Assert
            ret.Should().BeSameAs(enumerable);
            ret.Should().BeEquivalentTo(enumerable);
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
            var ret = list.WhereIf(true, x => x.Equals(SampleStrings.str1));

            // Assert
            ret.Should().HaveCount(1);
            ret.Should().BeEquivalentTo(listWithStr1);
            ret.Should().NotBeSameAs(listWithStr1);
            ret.Should().NotBeSameAs(list);
        }

        [Fact]
        public void WhereIf_WithFalse_SkipFilter()
        {
            // Arrange
            var list = SampleStrings.GetAsList();
            var originalListCount = list.Count;
            var listWithStr1 = new List<string> { SampleStrings.str1 };

            // Act
            var ret = list.WhereIf(false, x => x.Equals(SampleStrings.str1));

            // Assert
            ret.Should().HaveCount(originalListCount);
            ret.Should().NotBeEquivalentTo(listWithStr1);
            ret.Should().NotBeSameAs(listWithStr1);
            ret.Should().BeEquivalentTo(list);
            ret.Should().BeSameAs(list);
        }

        #endregion

    }
}
