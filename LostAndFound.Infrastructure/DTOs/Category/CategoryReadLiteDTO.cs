using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.Category
{
    public class CategoryReadLiteDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ItemValue Value { get; set; }
        public bool IsSensitive { get; set; }
    }
}
