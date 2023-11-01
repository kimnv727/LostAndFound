using LostAndFound.Infrastructure.DTOs.Category;
using System.Collections.Generic;

namespace LostAndFound.Infrastructure.DTOs.CategoryGroup
{
    public class CategoryGroupReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ICollection<CategoryReadLiteDTO> Categories { get; set; }
    }
}