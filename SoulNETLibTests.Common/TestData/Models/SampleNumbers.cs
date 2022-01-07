using System.Collections.Generic;

namespace SoulNETLibTests.Common.TestData.Models
{
    public static class SampleNumbers
    {
        public const int int1 = 9034245;
        public const int int2 = 1234534567;
        public const int int3 = 686743564;
        public const int int4 = 655655453;
        public const int int5 = 123234543;

        public static List<int> GetIntAsList()
        {
            return new List<int> { int1, int2, int3, int4, int5 };
        }
    }
}
