using System;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostReadDTO
    {
        public int Id { get; set; }
        public string PostUserId { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public string PostLocationLocationName { get; set; }
        public string PostCategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}