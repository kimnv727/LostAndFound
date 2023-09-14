using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Post;

namespace LostAndFound.Infrastructure.DTOs.PostFlag
{
    public class PostFlagReadDTO
    {
        public string UserId { get; set; }
        public PostReadDTO Post { get; set; }
        public PostFlagReason PostFlagReason { get; set; }
        public bool IsActive { get; set; }
    }
}