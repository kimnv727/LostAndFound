using System.Linq;

namespace LostAndFound.Infrastructure.DTOs.Property
{
    public class PropertyLiteReadDTO 
    {
        public int Id { get; set; }
        public string PropertyName { get; set; }
        public string Address { get; set; }
        public bool? IsActive { get; set; }
    }
}