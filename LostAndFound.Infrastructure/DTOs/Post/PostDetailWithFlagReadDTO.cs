using System;
using System.Collections.Generic;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Category;
using LostAndFound.Infrastructure.DTOs.Location;
using LostAndFound.Infrastructure.DTOs.PostMedia;
using LostAndFound.Infrastructure.DTOs.User;
using Newtonsoft.Json;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostDetailWithFlagReadDTO
    {
        public int Id { get; set; }

        public string PostUserId { get; set; }

        public string Title { get; set; }

        public string PostContent { get; set; }

        /*[JsonProperty(PropertyName = "locationName")]
        public string LocationLocationName { get; set; }

        public string CategoryName { get; set; }*/
        public string? PostLocation { get; set; }
        public string? PostCategory { get; set; }
        public string? LostDateFrom { get; set; }
        public string? LostDateTo { get; set; }

        public PostStatus PostStatus { get; set; }
        public int[]? PostLocationIdList => string.IsNullOrWhiteSpace(PostLocation) ? null : Array.ConvertAll(PostLocation.Substring(1, PostLocation.Length - 2).Split('|'), int.Parse);
        public int[]? PostCategoryIdList => string.IsNullOrWhiteSpace(PostCategory) ? null : Array.ConvertAll(PostCategory.Substring(1, PostCategory.Length - 2).Split('|'), int.Parse);
        public ICollection<LocationReadDTO> PostLocationList { get; set; }
        public ICollection<CategoryReadDTO> PostCategoryList { get; set; }
        public DateTime CreatedDate { get; set; }

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
        public LocationReadDTO Location { get; set; }
        public UserReadDTO User { get; set; }
        public ICollection<PostMediaReadDTO> PostMedias { get; set; }
    }
}