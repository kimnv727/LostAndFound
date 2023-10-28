using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.ItemClaim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IItemClaimService
    {
        public Task<PaginatedResponse<ItemClaimReadDTO>> QueryItemClaimAsync(ItemClaimQuery query);

        public Task<IEnumerable<ItemClaimReadDTO>> GetClaimsByItemIdAsync(int itemId);
        public Task<IEnumerable<ItemClaimReadDTO>> GetClaimsByUserIdAsync(string userId);
        public Task<ItemClaimReadDTO> ClaimAnItemAsync(int itemId, string userId);
        public Task UnClaimAnItemAsync(int itemId, string userId);
    }
}
