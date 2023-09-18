using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.CategoryGroup
{
    public class CategoryGroupQuery : PaginatedQuery
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}