using LostAndFound.Infrastructure.DTOs.User;

namespace LostAndFound.Infrastructure.DTOs.GiveawayParticipant
{
    public class GiveawayParticipantReadDTO
    {
        public int GiveawayId { get; set; }
        public UserReadDTO User { get; set; }
        public bool IsActive { get; set; }
        public bool IsWinner { get; set; }
        public bool IsChosenAsWinner { get; set; }
    }
}