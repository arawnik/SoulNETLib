using FluentAssertions;
using SoulNETLib.Common.Extension;
using SoulNETLibTests.Common.TestData.Models;
using System;
using Xunit;

namespace SoulNETLibTests.Extension
{
    public class ObjectExtensionTests
    {

        #region TypeHasProperty

        [Theory]
        [InlineData("name")]
        [InlineData("Name")]
        [InlineData("NAME")]
        [InlineData("comment")]
        [InlineData("Comment")]
        [InlineData("COMMENT")]
        [InlineData("dateTime")]
        [InlineData("DateTime")]
        [InlineData("DATETIME")]
        public void TypeHasProperty_ExistingPropertiesInsensitive_ReturnTrue(string propertyName)
        {
            // Arrange

            // Act
            var exists = typeof(SampleObject).HasProperty(propertyName, false);

            // Assert
            exists.Should().BeTrue();
        }

        [Theory]
        [InlineData("Name")]
        [InlineData("Comment")]
        [InlineData("DateTime")]
        [InlineData("trouble")]
        [InlineData("Trouble")]
        [InlineData("TROUBLE")]
        public void TypeHasProperty_ExistingPropertiesSensitive_ReturnTrue(string propertyName)
        {
            // Arrange

            // Act
            var exists = typeof(SampleObject).HasProperty(propertyName);

            // Assert
            exists.Should().BeTrue();
        }

        [Theory]
        [InlineData("name")]
        [InlineData("NAME")]
        [InlineData("comment")]
        [InlineData("dateTime")]
        public void TypeHasProperty_WrongCaseExistingPropertiesSensitive_ReturnFalse(string propertyName)
        {
            // Arrange

            // Act
            var exists = typeof(SampleObject).HasProperty(propertyName);

            // Assert
            exists.Should().BeFalse();
        }

        [Theory]
        [InlineData("name-")]
        [InlineData("N4ME")]
        [InlineData("nam")]
        [InlineData("namee")]
        [InlineData("iid")]
        [InlineData("property")]
        [InlineData("time")]
        [InlineData("DATE")]
        [InlineData("GetHidden")]
        public void TypeHasProperty_WrongProperties_ReturnFalse(string propertyName)
        {
            // Arrange

            // Act
            var exists = typeof(SampleObject).HasProperty(propertyName);

            // Assert
            exists.Should().BeFalse();
        }

        [Fact]
        public void TypeHasProperty_PrivateProperties_ReturnFalse()
        {
            // Arrange

            // Act
            var exists = typeof(SampleObject).HasProperty("hidden");

            // Assert
            exists.Should().BeFalse();
        }

        [Fact]
        public void TypeHasProperty_ProtectedProperties_ReturnFalse()
        {
            // Arrange

            // Act
            var exists = typeof(SampleObject).HasProperty("semi");

            // Assert
            exists.Should().BeFalse();
        }

        #endregion

        #region ObjectHasProperty

        [Theory]
        [InlineData("name")]
        [InlineData("Name")]
        [InlineData("NAME")]
        [InlineData("comment")]
        [InlineData("dateTime")]
        public void HasProperty_ClassExistingPropertiesInsensitive_ReturnTrue(string propertyName)
        {
            // Arrange
            var testObj = new SampleObject(SampleNumbers.int1, SampleStrings.str1, DateTime.UtcNow, SampleStrings.str2, SampleStrings.str3, SampleStrings.str4);

            // Act
            var exists = testObj.HasProperty(propertyName, false);

            // Assert
            exists.Should().BeTrue();
        }

        [Theory]
        [InlineData("Name")]
        [InlineData("Comment")]
        [InlineData("DateTime")]
        [InlineData("trouble")]
        [InlineData("Trouble")]
        [InlineData("TROUBLE")]
        public void HasProperty_ClassExistingPropertiesSensitive_ReturnTrue(string propertyName)
        {
            // Arrange
            var testObj = new SampleObject(SampleNumbers.int1, SampleStrings.str1, DateTime.UtcNow, SampleStrings.str2, SampleStrings.str3, SampleStrings.str4);

            // Act
            var exists = testObj.HasProperty(propertyName);

            // Assert
            exists.Should().BeTrue();
        }

        [Theory]
        [InlineData("name")]
        [InlineData("NAME")]
        [InlineData("comment")]
        [InlineData("dateTime")]
        public void HasProperty_ClassWrongCaseExistingPropertiesSensitive_ReturnFalse(string propertyName)
        {
            // Arrange
            var testObj = new SampleObject(SampleNumbers.int1, SampleStrings.str1, DateTime.UtcNow, SampleStrings.str2, SampleStrings.str3, SampleStrings.str4);

            // Act
            var exists = testObj.HasProperty(propertyName);

            // Assert
            exists.Should().BeFalse();
        }

        [Theory]
        [InlineData("name-")]
        [InlineData("N4ME")]
        [InlineData("nam")]
        [InlineData("namee")]
        [InlineData("iid")]
        [InlineData("property")]
        [InlineData("time")]
        [InlineData("DATE")]
        [InlineData("GetHidden")]
        public void TypeHasProperty_ClassWrongProperties_ReturnFalse(string propertyName)
        {
            // Arrange
            var testObj = new SampleObject(SampleNumbers.int1, SampleStrings.str1, DateTime.UtcNow, SampleStrings.str2, SampleStrings.str3, SampleStrings.str4);

            // Act
            var exists = testObj.HasProperty(propertyName);

            // Assert
            exists.Should().BeFalse();
        }


        [Theory]
        [InlineData("name")]
        [InlineData("Name")]
        [InlineData("NAME")]
        [InlineData("comment")]
        [InlineData("dateTime")]
        public void HasProperty_ObjectExistingPropertiesInsensitive_ReturnTrue(string propertyName)
        {
            // Arrange
            var testDynamic = new { id = SampleNumbers.int1, name = SampleStrings.str1, DateTime = DateTime.UtcNow, Comment = SampleStrings.str2, test = SampleStrings.str3 };
            object testObj = testDynamic;

            // Act
            var exists = testDynamic.HasProperty(propertyName, false);
            var existsInObj = testObj.HasProperty(propertyName, false);

            // Assert
            exists.Should().BeTrue();
            existsInObj.Should().BeTrue();
        }

        [Theory]
        [InlineData("Name")]
        [InlineData("NAME")]
        [InlineData("comment")]
        [InlineData("dateTime")]
        [InlineData("random")]
        public void HasProperty_ObjectWrongPropertiesSensitive_ReturnFalse(string propertyName)
        {
            // Arrange
            var testDynamic = new { id = SampleNumbers.int1, name = SampleStrings.str1, DateTime = DateTime.UtcNow, Comment = SampleStrings.str2, test = SampleStrings.str3 };
            object testObj = testDynamic;

            // Act
            var exists = testDynamic.HasProperty(propertyName);
            var existsInObj = testObj.HasProperty(propertyName);

            // Assert
            exists.Should().BeFalse();
            existsInObj.Should().BeFalse();
        }

        #endregion

        #region CopyPropertyValuesFrom

        [Fact]
        public void CopyPropertyValuesFrom_FieldsAreFound_ApplyChanges()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var testObj = new { Id = SampleNumbers.int1, Name = SampleStrings.str1, DateTime = now, Comment = SampleStrings.str2, Hidden = SampleStrings.str3, Semi = SampleStrings.str4 };
            var testClass = new SampleObject(SampleNumbers.int2, SampleStrings.str4, DateTime.Today, SampleStrings.str5, SampleStrings.str6, SampleStrings.str7);

            // Act
            testClass.CopyPropertyValuesFrom(testObj);

            // Assert
            testClass.Id.Should().Be(SampleNumbers.int1);
            testClass.Name.Should().Be(SampleStrings.str1);
            testClass.DateTime.Should().Be(now);
            testClass.Comment.Should().Be(SampleStrings.str2);
            testClass.GetHidden().Should().Be(SampleStrings.str6, "private properties cannot be altered.");
            testClass.GetSemi().Should().Be(SampleStrings.str7, "protected properties cannot be altered.");
        }

        [Fact]
        public void CopyPropertyValuesFrom_FieldsNotFound_DiscardChanges()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var today = DateTime.Today;
            var testObj = new { ID = SampleNumbers.int2, name = SampleStrings.str1, dateTime = now, comment = SampleStrings.str2, test = SampleStrings.str3, Trouble = SampleNumbers.int1 };
            var testClass = new SampleObject(SampleNumbers.int1, SampleStrings.str4, today, SampleStrings.str5, SampleStrings.str6, SampleStrings.str7);
            testClass.trouble = testClass.Trouble = testClass.TROUBLE = SampleNumbers.int4;

            // Act
            testClass.CopyPropertyValuesFrom(testObj);

            // Assert
            testClass.Id.Should().Be(SampleNumbers.int1);
            testClass.Name.Should().Be(SampleStrings.str4);
            testClass.DateTime.Should().Be(today);
            testClass.trouble.Should().Be(SampleNumbers.int4);
            testClass.Trouble.Should().Be(SampleNumbers.int1);
            testClass.TROUBLE.Should().Be(SampleNumbers.int4);
        }

        [Fact]
        public void CopyPropertyValuesFrom_NullDestination_KeepValues()
        {
            // Arrange
            var now = DateTime.UtcNow;
            object? testObj = null;
            var testClass = new SampleObject(SampleNumbers.int1, SampleStrings.str4, now, SampleStrings.str5, SampleStrings.str6, SampleStrings.str7);
            testClass.trouble = testClass.Trouble = testClass.TROUBLE = SampleNumbers.int4;

            // Act
            testClass.CopyPropertyValuesFrom(testObj);

            // Assert
            testClass.Id.Should().Be(SampleNumbers.int1);
            testClass.Name.Should().Be(SampleStrings.str4);
            testClass.DateTime.Should().Be(now);
            testClass.trouble.Should().Be(SampleNumbers.int4);
            testClass.Trouble.Should().Be(SampleNumbers.int4);
            testClass.TROUBLE.Should().Be(SampleNumbers.int4);
        }

        [Fact]
        public void CopyPropertyValuesFrom_NullSource_SourceStaysNull()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var testObj = new { Id = SampleNumbers.int2, name = SampleStrings.str1, dateTime = now, comment = SampleStrings.str2, test = SampleStrings.str3, Trouble = SampleNumbers.int1 };
            
            #pragma warning disable CS8600 // Testing null value to non-nullable type.
            SampleObject testClass = null;
            #pragma warning restore CS8600 

            // Act
            testClass.CopyPropertyValuesFrom(testObj);

            // Assert
            testClass.Should().BeNull("Source will not be initialized.");
        }

        #endregion

    }
}
