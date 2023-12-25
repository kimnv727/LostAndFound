using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Giveaway;
using LostAndFound.Infrastructure.DTOs.Item;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IGiveawayService
    {
        Task UpdateGiveawayStatusAsync(int giveawayId, GiveawayStatus giveawayStatus);
        Task<GiveawayReadDTO> GetGiveawayByIdAsync(int giveawayId);
        Task<GiveawayDetailWithParticipantsReadDTO> GetGiveawayIncludeParticipantsByIdAsync(int giveawayId);
        Task<PaginatedResponse<GiveawayReadDTO>> QueryGiveawayAsync(GiveawayQuery query);
        Task<PaginatedResponse<GiveawayReadDTO>> QueryGiveawayWithStatusAsync(GiveawayQueryWithStatus query);
        Task<GiveawayReadDTO> CreateGiveawayAsync(GiveawayWriteDTO giveawayWriteDTO);
        Task<GiveawayReadDTO> UpdateGiveawayDetailsAsync(int giveawayId, GiveawayUpdateDTO giveawayUpdateDTO);
        Task<IEnumerable<ItemReadDTO>> ListItemsSuitableForGiveawayAsync();
    }
}