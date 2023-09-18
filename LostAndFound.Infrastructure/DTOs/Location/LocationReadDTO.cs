namespace LostAndFound.Infrastructure.DTOs.Location
{
    public class LocationReadDTO
    {
        public int Id { get; set; }
        
        public int PropertyId { get; set; }
        
        public string LocationName { get; set; }
        
        public int Floor { get; set; }
    }
}