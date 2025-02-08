using SoulNETLib.EFCore.Collection;
using SoulNETLib.Common.Extension;
using SoulNETLibTests.Common.TestData.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;

namespace SoulNETLib.EFCoreTests.Collection
{
    public class PaginatedListTests
    {
        private readonly string _pathToCommonTestFiles = "..\\..\\..\\..\\SoulNETLibTests.Common\\TestData\\Files\\";

        [Fact]
        public void PaginatedList_InitializedList_ReturnJsonList()
        {
            // Arrange
            var items = new List<SampleObject>()
            {
                new("name1") {Id =1, Comment = "comment1"},
                new("name2") {Id =2, Comment = "comment2"},
                new("name3") {Id =3, Comment = "comment3"},
                new("name4") {Id =4, Comment = "comment4"},
                new("name5") {Id =5, Comment = "comment5"}
            };
            var list = new PaginatedList<SampleObject>(items, 123123, 2, 5);

            // Act
            string jsonString = JsonSerializer.Serialize(list).RemoveWhitespaces();

            // Assert
            Assert.Equal("""[{"Id":1,"Name":"name1","Comment":"comment1","DateTime":"0001-01-01T00:00:00"},{"Id":2,"Name":"name2","Comment":"comment2","DateTime":"0001-01-01T00:00:00"},{"Id":3,"Name":"name3","Comment":"comment3","DateTime":"0001-01-01T00:00:00"},{"Id":4,"Name":"name4","Comment":"comment4","DateTime":"0001-01-01T00:00:00"},{"Id":5,"Name":"name5","Comment":"comment5","DateTime":"0001-01-01T00:00:00"}]""", jsonString);
        }

        [Fact]
        public void PaginatedList_InitializedList_MatchJsonSample()
        {
            // Arrange
            var items = new List<SampleObject>()
            {
                new("name1") {Id =1, Comment = "comment1"},
                new("name2") {Id =2, Comment = "comment2"},
                new("name3") {Id =3, Comment = "comment3"},
                new("name4") {Id =4, Comment = "comment4"},
                new("name5") {Id =5, Comment = "comment5"}
            };
            var list = new PaginatedList<SampleObject>(items, 123123, 2, 5);

            // Act
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
            string jsonString = JsonSerializer.Serialize(list, options);
            string contents = File.ReadAllText(_pathToCommonTestFiles + "PaginatedListSample.json");

            // Assert
            Assert.Equal(contents, jsonString);
        }

        [Fact]
        public void PaginatedResult_InitializedList_ReturnJsonResult()
        {
            // Arrange
            var items = new List<SampleObject>()
            {
                new("name1") {Id =1, Comment = "comment1"},
                new("name2") {Id =2, Comment = "comment2"},
                new("name3") {Id =3, Comment = "comment3"},
                new("name4") {Id =4, Comment = "comment4"},
                new("name5") {Id =5, Comment = "comment5"}
            };
            var list = new PaginatedResult<SampleObject>(new PaginatedList<SampleObject>(items, 123123, 2, 5));

            // Act
            string jsonString = JsonSerializer.Serialize(list).RemoveWhitespaces();

            // Assert
            Assert.Equal("{\"page\":2,\"size\":5,\"pages\":24625,\"rows\":123123,\"items\":[{\"Id\":1,\"Name\":\"name1\",\"Comment\":\"comment1\",\"DateTime\":\"0001-01-01T00:00:00\"},{\"Id\":2,\"Name\":\"name2\",\"Comment\":\"comment2\",\"DateTime\":\"0001-01-01T00:00:00\"},{\"Id\":3,\"Name\":\"name3\",\"Comment\":\"comment3\",\"DateTime\":\"0001-01-01T00:00:00\"},{\"Id\":4,\"Name\":\"name4\",\"Comment\":\"comment4\",\"DateTime\":\"0001-01-01T00:00:00\"},{\"Id\":5,\"Name\":\"name5\",\"Comment\":\"comment5\",\"DateTime\":\"0001-01-01T00:00:00\"}]}", jsonString);
        
        }

        [Fact]
        public void PaginatedResult_InitializedList_MatchJsonSample()
        {
            // Arrange
            var items = new List<SampleObject>()
            {
                new("name1") {Id =1, Comment = "comment1"},
                new("name2") {Id =2, Comment = "comment2"},
                new("name3") {Id =3, Comment = "comment3"},
                new("name4") {Id =4, Comment = "comment4"},
                new("name5") {Id =5, Comment = "comment5"}
            };
            var list = new PaginatedResult<SampleObject>(new PaginatedList<SampleObject>(items, 123123, 2, 5));

            // Act
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
            string jsonString = JsonSerializer.Serialize(list, options);
            string contents = File.ReadAllText(_pathToCommonTestFiles + "PaginatedResultSample.json");

            // Assert
            Assert.Equal(contents, jsonString);
        }
    }
}
