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
            Assert.Equal(1, ret.Count());
            Assert.Equal(queryableWithStr1, ret);
            Assert.NotSame(queryableWithStr1, ret);
            Assert.NotSame(queryable, ret);
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
            Assert.Equal(originalListCount, ret.Count());
            Assert.NotEqual(queryableWithStr1, ret);
            Assert.NotSame(queryableWithStr1, ret);
            Assert.Equal(queryable, ret);
            Assert.Same(queryable, ret);
        }

        #endregion

    }
}
