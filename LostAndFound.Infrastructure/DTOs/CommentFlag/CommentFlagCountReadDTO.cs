using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.CommentFlag
{
    public class CommentFlagCountReadDTO
    {
        [JsonProperty(PropertyName = "Wrong Information")]
        public int WrongInformationCount { get; set; }
        [JsonProperty(PropertyName = "Violated User")]
        public int ViolatedUserCount { get; set; }
        [JsonProperty(PropertyName = "Spam")]
        public int SpamCount { get; set; }
        [JsonProperty(PropertyName = "Others")]
        public int OthersCount { get; set; }
        [JsonProperty(PropertyName = "Total Count")]
        public int TotalCount { get; set; }
    }
}
