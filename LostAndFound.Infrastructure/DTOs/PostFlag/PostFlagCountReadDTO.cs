﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.PostFlag
{
    public class PostFlagCountReadDTO
    {
        [JsonProperty(PropertyName = "wrongInformation")]
        public int WrongInformationCount { get; set; }
        [JsonProperty(PropertyName = "violatedUser")]
        public int ViolatedUserCount { get; set; }
        [JsonProperty(PropertyName = "spam")]
        public int SpamCount { get; set; }
        [JsonProperty(PropertyName = "others")]
        public int OthersCount { get; set; }
        [JsonProperty(PropertyName = "totalCount")]
        public int TotalCount { get; set; }

    }
}
