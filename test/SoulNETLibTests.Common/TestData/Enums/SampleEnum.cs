using System.ComponentModel;
using System.Runtime.Serialization;
using SoulNETLibTests.Common.TestData.Models;

namespace SoulNETLibTests.Common.TestData.Enums;

/// <summary>
/// Sample enum values. Series:
/// 0:      Zero default is missing
/// 1-9:    Fully kitted values
/// 10-19:  No descriptions
/// </summary>
#pragma warning disable CA1008 // Enums should have zero value
public enum SampleEnum
#pragma warning restore CA1008 // Enums should have zero value
{
    [Description(SampleStrings.str1)]
    [EnumMember(Value = "ONE")]
    One = 1,

    [Description(SampleStrings.str2)]
    [EnumMember(Value = "TWO")]
    Two = 2,

    [Description(SampleStrings.str3)]
    Three = 3,

    [Description(SampleStrings.str4)]
    Four = 4,

    [Description(SampleStrings.str5)]
    Five = 5,
    Ten = 10,
    Eleven = 11,
    Twelve = 12,
}
