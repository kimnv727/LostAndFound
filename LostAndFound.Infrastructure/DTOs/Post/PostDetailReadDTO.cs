using System;
using System.Collections.Generic;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.PostMedia;
using LostAndFound.Infrastructure.DTOs.User;
using Newtonsoft.Json;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostDetailReadDTO
    {
        public int Id { get; set; }

        public string PostUserId { get; set; }

        public string Title { get; set; }

        public string PostContent { get; set; }

        [JsonProperty(PropertyName = "LocationName")]
        public string LocationLocationName { get; set; }

        public string CategoryName { get; set; }

        public PostStatus PostStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public UserReadDTO User { get; set; }
        public ICollection<PostMediaReadDTO> PostMedias { get; set; }
    }
}