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
    public class PostReadDTO
    {
        public int Id { get; set; }
        public string PostUserId { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }

        /*[JsonProperty(PropertyName = "locationName")]
        public string LocationLocationName { get; set; }
        public int LocationId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }*/
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
        public UserReadDTO User { get; set; }
        public ICollection<PostMediaLiteReadDTO> PostMedias { get; set; }
    }
}