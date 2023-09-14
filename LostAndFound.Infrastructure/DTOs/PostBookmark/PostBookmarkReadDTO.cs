using LostAndFound.Infrastructure.DTOs.Post;

namespace LostAndFound.Infrastructure.DTOs.PostBookmark
{
    public class PostBookmarkReadDTO
    {
        public string UserId { get; set; }
        public PostReadDTO Post { get; set; }
        public bool IsActive { get; set; }
    }
}