using LostAndFound.Infrastructure.DTOs.Media;


namespace LostAndFound.Infrastructure.DTOs.ItemMedia
{
    public class ItemMediaReadDTO
    {
        public int ItemId { get; set; }
        public MediaReadDTO Media { get; set; }
    }
}
