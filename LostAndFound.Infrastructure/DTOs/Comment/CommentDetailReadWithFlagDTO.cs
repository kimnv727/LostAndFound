using LostAndFound.Infrastructure.DTOs.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Comment
{
    public class CommentDetailReadWithFlagDTO
    {
        public int Id { get; set; }
        public string CommentUserId { get; set; }
        public int PostId { get; set; }
        public string CommentContent { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public UserReadDTO User { get; set; }

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
