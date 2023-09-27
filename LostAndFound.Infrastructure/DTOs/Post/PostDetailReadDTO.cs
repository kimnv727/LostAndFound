using System;
using System.Collections.Generic;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.PostMedia;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostDetailReadDTO
    {
        public int Id { get; set; }
        public string PostUserId { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public string PostLocationLocationName { get; set; }
        public string PostCategoryName { get; set; }
        public PostStatus PostStatus { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<PostMediaReadDTO> PostMedias { get; set; }
    }
}