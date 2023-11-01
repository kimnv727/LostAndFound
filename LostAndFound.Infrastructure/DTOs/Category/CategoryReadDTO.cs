using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.CategoryGroup;

namespace LostAndFound.Infrastructure.DTOs.Category
{
    public class CategoryReadDTO 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ItemValue Value { get; set; }
        public bool IsSensitive { get; set; }
        public CategoryGroupReadLiteDTO CategoryGroup { get; set; }
    }
}
