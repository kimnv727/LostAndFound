using LostAndFound.Infrastructure.DTOs.Property;
using Newtonsoft.Json;

namespace LostAndFound.Infrastructure.DTOs.Location
{
    public class LocationReadDTO
    {
        public int Id { get; set; }
        
        public int PropertyId { get; set; }
        
        public string LocationName { get; set; }
        
        public int Floor { get; set; }
        public bool? IsActive { get; set; }

        public CampusLiteReadDTO Property { get; set; }
    }
}