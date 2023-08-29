using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.Media
{
    public class MediaQuery : PaginatedQuery
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
