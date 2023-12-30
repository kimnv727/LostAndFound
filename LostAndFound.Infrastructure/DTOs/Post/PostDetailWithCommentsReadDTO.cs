using System;
using System.Collections.Generic;
using System.Linq;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Category;
using LostAndFound.Infrastructure.DTOs.Comment;
using LostAndFound.Infrastructure.DTOs.Location;
using LostAndFound.Infrastructure.DTOs.PostMedia;
using LostAndFound.Infrastructure.DTOs.User;
using Newtonsoft.Json;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostDetailWithCommentsReadDTO
    {
        public int Id { get; set; }
        public string PostUserId { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public string? LostDateFrom { get; set; }
        public string? LostDateTo { get; set; }
        public int[]? PostLocationIdList => this.Locations.Select(l => l.Id).ToArray();
        public int[]? PostCategoryIdList => this.Categories.Select(c => c.Id).ToArray();
        public ICollection<LocationReadDTO> PostLocationList => this.Locations;
        public ICollection<CategoryReadDTO> PostCategoryList => this.Categories;
        public PostStatus PostStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserReadDTO User { get; set; }
        public ICollection<PostMediaReadDTO> PostMedias { get; set; }
        public ICollection<CommentReadDTO> Comments { get; set; }
        [JsonIgnore]
        public ICollection<LocationReadDTO> Locations { get; set; }
        [JsonIgnore]
        public ICollection<CategoryReadDTO> Categories { get; set; }
    }
}