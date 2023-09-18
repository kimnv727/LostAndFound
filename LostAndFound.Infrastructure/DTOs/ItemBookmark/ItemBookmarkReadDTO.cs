using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.ItemBookmark
{
    public class ItemBookmarkReadDTO
    {
        public int ItemId { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }
    }
}