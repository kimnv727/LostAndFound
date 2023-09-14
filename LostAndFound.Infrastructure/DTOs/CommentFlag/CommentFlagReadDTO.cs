using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Comment;

namespace LostAndFound.Infrastructure.DTOs.CommentFlag
{
    public class CommentFlagReadDTO
    {
        public string UserId { get; set; }
        public CommentReadDTO Comment { get; set; }
        public CommentFlagReason CommentFlagReason { get; set; }
        public bool IsActive { get; set; }
    }
}