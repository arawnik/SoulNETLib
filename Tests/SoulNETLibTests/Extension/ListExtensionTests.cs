using FluentAssertions;
using SoulNETLib.Common.Extension;
using SoulNETLibTests.Common.TestData.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace SoulNETLibTests.Extension
{
    public class ListExtensionTests
    {
        [Fact]
        public void Replace_CorrectMatch_ReplacesValue()
        {
            // Arrange
            var list = new List<TestObj>
            {
                new(){ id = 1, str = SampleStrings.str1 },
                new(){ id = 2, str = SampleStrings.str2 },
                new(){ id = 3, str = SampleStrings.str3 },
                new(){ id = 4, str = SampleStrings.str4 },
                new(){ id = 5, str = SampleStrings.str5 },
                new(){ id = 6, str = SampleStrings.str6 },
            };

            // Act
            var ret = list.Replace((to) => to.id == 1, new() { id = 1, str = SampleStrings.str7 });

            // Assert
            ret.Should().Be(0);
            list[ret].str.Should().Be(SampleStrings.str7);
            list[ret].id.Should().Be(1);
            list.Count.Should().Be(6);
        }

        [Fact]
        public void Replace_IncorrectMatch_ReturnsNegative()
        {
            // Arrange
            var list = new List<TestObj>
            {
                new(){ id = 1, str = SampleStrings.str1 },
                new(){ id = 2, str = SampleStrings.str2 },
                new(){ id = 3, str = SampleStrings.str3 },
                new(){ id = 4, str = SampleStrings.str4 },
                new(){ id = 5, str = SampleStrings.str5 },
                new(){ id = 6, str = SampleStrings.str6 },
            };

            // Act
            var ret = list.Replace((to) => to.id == 69, new() { id = 69, str = SampleStrings.str7 });

            // Assert
            ret.Should().Be(-1);
        }

        private sealed class TestObj
        {
            public int id;
            public string? str;
        }
    }
}
