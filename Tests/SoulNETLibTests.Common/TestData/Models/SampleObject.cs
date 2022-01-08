using System;
using System.Text.Json.Serialization;

namespace SoulNETLibTests.Common.TestData.Models
{
    public class SampleObject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Comment { get; set; }
        public DateTime DateTime { get; set; }

        private string? Hidden { get; set;}

        protected string? Semi { get; set; }


        [JsonIgnore]
        public int trouble { get; set; }
        [JsonIgnore]
        public int Trouble { get; set; }
        [JsonIgnore]
        public int TROUBLE { get; set; }

        public SampleObject(string name)
        {
            Name = name;
        }
        public SampleObject(int id, string name, DateTime dateTime, string? comment = null, string? hidden = null, string? semi = null)
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
}
