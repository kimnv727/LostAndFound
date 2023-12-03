using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemUpdateWithoutCabinetIdDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int LocationId { get; set; }
        public int CategoryId { get; set; }
        public string? FoundDate { get; set; }

        //public ItemStatus? ItemStatus { get; set; }
    }
}