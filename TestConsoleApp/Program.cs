// Arrange
using SoulNETLib.EFCore.Collection;
using SoulNETLibTests.Common.TestData.Models;
using System.Text.Json;

var items = new List<SampleObject>()
            {
                new SampleObject("name1") {Id =1, Comment = "comment1"},
                new SampleObject("name2") {Id =2, Comment = "comment2"},
                new SampleObject("name3") {Id =3, Comment = "comment3"},
                new SampleObject("name4") {Id =4, Comment = "comment4"},
                new SampleObject("name5") {Id =5, Comment = "comment5"}
            };
//var list = new PaginatedList<SampleObject>(items, 123123, 2, 5);
var list = new PaginatedResult<SampleObject>(new PaginatedList<SampleObject>(items, 123123, 2, 5));

// Act
var options = new JsonSerializerOptions
{
    WriteIndented = true,
};
string jsonString = JsonSerializer.Serialize(list, options);

Console.WriteLine(jsonString);