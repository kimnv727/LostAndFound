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
        [JsonProperty(PropertyName = "falseInformation")]
        public int FalseInformationCount { get; set; }
        [JsonProperty(PropertyName = "violatedUserPolicies")]
        public int ViolatedUserPoliciesCount { get; set; }
        [JsonProperty(PropertyName = "spam")]
        public int SpamCount { get; set; }
        [JsonProperty(PropertyName = "totalCount")]
        public int TotalCount { get; set; }
    }
}
