namespace SoulNETLibTests.Common.TestData.Models;

public static class SampleStrings
{
    public const string str1 = "Lorem ipsum dolor sit amet";
    public const string str2 = "consectetur adipiscing elit";
    public const string str3 = "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua";
    public const string str4 = "A arcu cursus vitae congue";
    public const string str5 = "Netus et malesuada fames ac turpis";
    public const string str6 = "Excepteur sint occaecat cupidatat non proident";
    public const string str7 = "sunt in culpa qui officia deserunt mollit anim id est laborum";

    public static ICollection<string> GetAsList()
    {
        return [str1, str2, str3, str4, str5, str6, str7];
    }
}
