using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Giveaway;
using LostAndFound.Infrastructure.DTOs.Notification;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IGiveawayRepository :
        IGetAllAsync<Giveaway>,
        IAddAsync<Giveaway>,
        IUpdate<Giveaway>,
        IDelete<Giveaway>
    {
        Task<Giveaway> FindGiveawayByIdAsync(int id);
        Task<Giveaway> FindGiveawayIncludeParticipantsAsync(int id);
        Task<IEnumerable<Giveaway>> QueryGiveawayAsync(GiveawayQuery query, bool trackChanges = false);
        Task<IEnumerable<Giveaway>> QueryGiveawayExcludeNotstartedAsync(GiveawayQueryExcludeNotStarted query, bool trackChanges = false);
        Task<IEnumerable<Giveaway>> QueryGiveawayWithStatusAsync(GiveawayQueryWithStatus query, bool trackChanges = false);
        public Task UpdateGiveawayRange(Giveaway[] giveaway);
        public Task<IEnumerable<Giveaway>> GetAllOngoingGiveaways();
        public Task<IEnumerable<Giveaway>> GetAllNotStartedGiveaways();
        public Task<IEnumerable<Giveaway>> GetAllWaitingGiveaways();
        Task PushNotificationForGiveawayResult(PushNotification notification);
        public Task<IEnumerable<Item>> GetAllItemsSuitableForGiveaway();
        public Task<IEnumerable<Giveaway>> CountFinishedGiveaways(int month, int year);
    }
}