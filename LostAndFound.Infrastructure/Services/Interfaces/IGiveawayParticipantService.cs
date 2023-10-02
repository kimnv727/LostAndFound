using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.GiveawayParticipant;
using LostAndFound.Infrastructure.DTOs.User;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IGiveawayParticipantService
    {
        public Task<int> CountGiveawayParticipantsAsync(int giveawayId);
        public Task<GiveawayParticipantReadDTO> GetGiveawayParticipant(int giveawayId, string userId);
        public Task<IEnumerable<UserReadDTO>> GetUsersParticipateInGiveaway(int giveawayId);
        public Task<GiveawayParticipantReadDTO> ParticipateInGiveaway(string userId, int giveawayId);
    }
}