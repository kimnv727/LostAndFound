using System;

namespace LostAndFound.Infrastructure.DTOs.Comment
{
    public class CommentReadDTO
    {
        public int Id { get; set; }
        public string CommentUserId { get; set; }
        public int PostId { get; set; }
        public string CommentContent { get; set; }
        public string CommentPath { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}