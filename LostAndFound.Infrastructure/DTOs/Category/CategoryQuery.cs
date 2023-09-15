using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.Category
{
    public class CategoryQuery : PaginatedQuery
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}