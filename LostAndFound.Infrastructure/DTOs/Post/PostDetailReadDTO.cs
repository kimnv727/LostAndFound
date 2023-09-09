using System;
using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostDetailReadDTO
    {
        public int Id { get; set; }
        public string PostUserId { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        
        public PostStatus PostStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        
        //TODO: Add Read CommentDTO here
    }
}