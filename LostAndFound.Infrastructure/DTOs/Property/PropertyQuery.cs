using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.Property
{
    public class PropertyQuery : PaginatedQuery
    {
        public string PropertyName { get; set; }
        public string Address { get; set; }
        public bool? IsActive { get; set; }
    }
}