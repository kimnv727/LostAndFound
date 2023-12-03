using LostAndFound.Core.Enums;
using System.Linq;

namespace LostAndFound.Infrastructure.DTOs.Property
{
    public class CampusLiteReadDTO 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool? IsActive { get; set; }
        public CampusLocation CampusLocation { get; set; }
    }
}