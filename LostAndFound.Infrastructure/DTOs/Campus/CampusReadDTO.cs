using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Location;
using System.Collections.Generic;
using System.Linq;

namespace LostAndFound.Infrastructure.DTOs.Property
{
    public class CampusReadDTO 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool? IsActive { get; set; }
        //public CampusLocation CampusLocation { get; set; }
        public ICollection<LocationLiteReadDTO> Locations { get; set; }
    }
}