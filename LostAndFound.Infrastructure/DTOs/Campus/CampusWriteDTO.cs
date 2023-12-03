using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.Property
{
    public class CampusWriteDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public CampusLocation CampusLocation { get; set; }
    }
}