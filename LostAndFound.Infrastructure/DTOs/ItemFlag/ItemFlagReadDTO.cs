using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.ItemFlag
{
    public class ItemFlagReadDTO
    {
        public int ItemId { get; set; }
        public string UserId { get; set; }
        public ItemFlagReason ItemFlagReason { get; set; }
        public bool IsActive { get; set; }
    }
}