using System;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostReadDTO
    {
        public string PostUserId { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}