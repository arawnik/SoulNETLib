using System.Text.Json.Serialization;

namespace SoulNETLibTests.Common.TestData.Models;

#pragma warning disable CA1708 // Identifiers should differ by more than case
public class SampleObject
#pragma warning restore CA1708 // Identifiers should differ by more than case
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string? Comment { get; set; }
    public DateTime DateTime { get; set; }

    private string? Hidden { get; set; }

#pragma warning disable CA1721 // Property names should not match get methods
    protected string? Semi { get; set; }
#pragma warning restore CA1721 // Property names should not match get methods

    [JsonIgnore]
#pragma warning disable IDE1006 // Naming Styles
    public int trouble { get; set; }
#pragma warning restore IDE1006 // Naming Styles

    [JsonIgnore]
    public int Trouble { get; set; }

    [JsonIgnore]
    public int TROUBLE { get; set; }

    public SampleObject(string name)
    {
        Name = name;
    }

    public SampleObject(
        int id,
        string name,
        DateTime dateTime,
        string? comment = null,
        string? hidden = null,
        string? semi = null
    )
    {
        Id = id;
        Name = name;
        Comment = comment;
        DateTime = dateTime;
        Hidden = hidden;
        Semi = semi;
    }

    public string? GetHidden()
    {
        return Hidden;
    }

    public string? GetSemi()
    {
        return Semi;
    }
}
