using LostAndFound.Infrastructure.DTOs.Media;

namespace LostAndFound.Infrastructure.DTOs.UserMedia
{
    public class UserMediaReadDTO
    {
        public string userId { get; set; }
        public MediaReadDTO Media { get; set; }
    }
}
