using FluentAssertions;
using SoulNETLib.Common.Extension;
using SoulNETLibTests.Common.TestData.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SoulNETLibTests.Extension
{
    public class IQueryableExtensionTests
    {

        #region WhereIf

        [Fact]
        public void WhereIf_WithTrue_ApplyFilter()
        {
            // Arrange
            var queryable = SampleStrings.GetAsList().AsQueryable();
            var queryableWithStr1 = new List<string> { SampleStrings.str1 }.AsQueryable();

            // Act
            var ret = queryable.WhereIf(true, x => x.Equals(SampleStrings.str1));

            // Assert
            ret.Should().HaveCount(1);
            ret.Should().BeEquivalentTo(queryableWithStr1);
            ret.Should().NotBeSameAs(queryableWithStr1);
            ret.Should().NotBeSameAs(queryable);
        }

        [Fact]
        public void WhereIf_WithFalse_SkipFilter()
        {
            // Arrange
            var queryable = SampleStrings.GetAsList().AsQueryable();
            var originalListCount = queryable.Count();
            var queryableWithStr1 = new List<string> { SampleStrings.str1 }.AsQueryable();

            // Act
            var ret = queryable.WhereIf(false, x => x.Equals(SampleStrings.str1));

            // Assert
            ret.Should().HaveCount(originalListCount);
            ret.Should().NotBeEquivalentTo(queryableWithStr1);
            ret.Should().NotBeSameAs(queryableWithStr1);
            ret.Should().BeEquivalentTo(queryable);
            ret.Should().BeSameAs(queryable);
        }

        #endregion

    }
}
