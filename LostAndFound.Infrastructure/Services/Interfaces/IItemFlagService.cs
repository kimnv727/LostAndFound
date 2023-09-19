using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.ItemFlag;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IItemFlagService
    {
        public Task<int> CountItemFlagAsync(int itemId);
        public Task<ItemFlagReadDTO> GetItemFlag(string userId, int itemId);
        public Task<IEnumerable<ItemReadDTO>> GetOwnItemFlags(string userId);
        public Task<ItemFlagReadDTO> FlagAnItem(string userId, int itemId, ItemFlagReason reason);
    }
}