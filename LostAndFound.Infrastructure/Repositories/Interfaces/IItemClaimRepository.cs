using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.ItemClaim;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IItemClaimRepository : 
        IGetAllAsync<ItemClaim>,
        IAddAsync<ItemClaim>,
        IUpdate<ItemClaim>,
        IDelete<ItemClaim>
    {
        public Task<IEnumerable<ItemClaim>> QueryItemClaimsAsync(ItemClaimQuery query, bool trackChanges = false);
        public Task<IEnumerable<ItemClaim>> GetAllClaimsByUserIdAsync(string userId);
        public Task<IEnumerable<ItemClaim>> GetAllActiveClaimsByUserIdAsync(string userId);
        public Task<IEnumerable<ItemClaim>> GetAllClaimsByItemIdAsync(int itemId);
        public Task<ItemClaim> FindClaimByItemIdAndUserId(int itemId, string userId);
        public Task<IEnumerable<ItemClaim>> GetAllActiveClaimsByItemIdAsync(int itemId);
    }
}
