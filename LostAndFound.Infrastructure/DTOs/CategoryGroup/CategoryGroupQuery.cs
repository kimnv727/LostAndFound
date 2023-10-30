using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.CategoryGroup
{
    public class CategoryGroupQuery : PaginatedQuery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemValue Value { get; set; }

        public bool IsSensitive { get; set; }
    }
}