using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.Property
{
    public class CampusQuery : PaginatedQuery
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public bool? IsActive { get; set; }
    }
}