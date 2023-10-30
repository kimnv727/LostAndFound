using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.CategoryGroup
{
    public class CategoryGroupWriteDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemValue Value { get; set; }
        public bool IsSensitive { get; set; }
    }
}