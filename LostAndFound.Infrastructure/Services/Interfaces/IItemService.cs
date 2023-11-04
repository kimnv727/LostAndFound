using LostAndFound.Infrastructure.DTOs.Item;
using System;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;
using System.Collections.Generic;
using LostAndFound.Core.Entities;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IItemService
    {
        Task UpdateItemStatusAsync(int itemId);
        /*Task<ItemReadDTO> UpdateItemDetail(int itemId, ItemUpdateWriteDTO itemUpdateWriteDTO);*/
        Task DeleteItemAsync(int itemId);
        Task<ItemDetailReadDTO> FindItemByIdAsync(int itemId);
        Task<ItemReadDTO> FindItemByNameAsync(String name);
        Task<DTOs.Common.PaginatedResponse<ItemReadDTO>> QueryItemAsync(ItemQueryWithStatus query);
        //Task<DTOs.Common.PaginatedResponse<ItemReadDTO>> QueryItemIgnoreStatusAsync(ItemQuery query);
        Task<bool> CheckItemFounderAsync(int itemId, string userId);
        Task<ItemReadDTO> UpdateItemDetailsAsync(int itemId, ItemUpdateDTO itemUpdateDTO);
        public Task<ItemReadDTO> CreateItemAsync(string userId, ItemWriteDTO itemWriteDTO);
        //public Task<ItemReadDTO> CreateItemAsync(ItemValue itemValue, string categoryName, string userId, ItemWriteDTO itemWriteDTO);
        public Task ChangeItemStatusAsync(int itemId, ItemStatus itemStatus);
        public Task<IEnumerable<ItemReadWithClaimStatusDTO>> GetClaimsForMember(string userId);
        public Task<IEnumerable<ItemReadWithClaimStatusDTO>> GetAllClaimsForManager();
        public Task<ItemReadWithClaimStatusDTO> GetAnItemWithClaimsForFounder(string userId, int itemId);
        public Task<ItemReadWithClaimStatusDTO> GetAnItemWithClaimsForMember(string userId, int itemId);
        public Task<ItemReadWithClaimStatusDTO> GetAnItemWithClaimsForManager(int itemId);
        public Task<ItemReadDTO> UpdateItemCabinet(int itemId, int cabinetId);

        public Task UpdateClaimStatusAsync(int itemId, string userId);

        public Task AcceptAClaimAsync(int itemId, string userId);
    }
}
