using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IGiveawayParticipantRepository :
        IGetAllAsync<GiveawayParticipant>,
        IAddAsync<GiveawayParticipant>,
        IUpdate<GiveawayParticipant>,
        IDelete<GiveawayParticipant>
    {
        Task<int> CountGiveawayParticipantsAsync(int giveawayId);
        Task<GiveawayParticipant> FindGiveawayParticipantAsync(int giveawayId, string userId);
        Task<IEnumerable<User>> FindUsersParticipateByGiveawayIdAsync(int giveawayId);
        Task<GiveawayParticipant> RandomizeGiveawayWinnerAsync(int giveawayId);
        public Task UpdateGiveawayParticipantRange(GiveawayParticipant[] giveawayParticipant);
    }
}