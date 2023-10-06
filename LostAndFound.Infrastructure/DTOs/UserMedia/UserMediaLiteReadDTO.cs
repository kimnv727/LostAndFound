using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Media;

namespace LostAndFound.Infrastructure.DTOs.UserMedia
{
    public class UserMediaLiteReadDTO
    {
        public UserMediaType MediaType { get; set; }
        public MediaReadDTO Media { get; set; }
    }
}
