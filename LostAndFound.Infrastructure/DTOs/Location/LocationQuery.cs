using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.Location
{
    public class LocationQuery : PaginatedQuery
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public int Floor { get; set; }
    }
}