using SoulNETLibTests.Common.TestData.Models;
using System.ComponentModel;

namespace SoulNETLibTests.Common.TestData.Enums
{
    /// <summary>
    /// Sample enum values. Series:
    /// 1-9:    Fully kitted values
    /// 10-19:  No descriptions
    /// </summary>
    public enum SampleEnum
    {
        [Description(SampleStrings.str1)]
        One = 1,
        [Description(SampleStrings.str2)]
        Two = 2,
        [Description(SampleStrings.str3)]
        Three = 3,
        [Description(SampleStrings.str4)]
        Four = 4,
        [Description(SampleStrings.str5)]
        Five = 5,
        Ten = 10,
        Eleven = 11,
        Twelve = 12
    }
}
