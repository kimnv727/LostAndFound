using LostAndFound.Infrastructure.DTOs.Media;


namespace LostAndFound.Infrastructure.DTOs.PostMedia
{
    public class PostMediaReadDTO
    {
        public int PostId { get; set; }
        public MediaReadDTO Media { get; set; }
    }
}
