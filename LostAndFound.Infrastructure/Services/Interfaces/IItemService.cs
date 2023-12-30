using LostAndFound.Infrastructure.DTOs.Item;
using System;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;
using System.Collections.Generic;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.ItemClaim;
using LostAndFound.Infrastructure.DTOs.Common;
using Microsoft.AspNetCore.Http;
using LostAndFound.Infrastructure.DTOs.Receipt;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IItemService
    {
        Task UpdateItemStatusAsync(int itemId);
        /*Task<ItemReadDTO> UpdateItemDetail(int itemId, ItemUpdateWriteDTO itemUpdateWriteDTO);*/
        Task DeleteItemAsync(int itemId);
        Task<ItemDetailReadDTO> FindItemByIdAsync(int itemId);
        Task<ItemReadDTO> FindItemByNameAsync(string name);
        Task<PaginatedResponse<ItemReadDTO>> QueryItemAsync(ItemQueryWithStatus query);
        Task<PaginatedResponse<ItemReadDTO>> QueryItemIgnorePendingRejectedAsync(ItemQueryIgnoreStatusExcludePendingRejected query);
        Task<PaginatedResponse<ItemDetailWithFlagReadDTO>> QueryItemIgnorePendingRejectedWithFlagAsync(ItemQueryWithFlag query);
        //Task<DTOs.Common.PaginatedResponse<ItemReadDTO>> QueryItemIgnoreStatusAsync(ItemQuery query);
        Task<bool> CheckItemFounderAsync(int itemId, string userId);
        Task<ItemReadDTO> UpdateItemDetailsAsync(int itemId, ItemUpdateDTO itemUpdateDTO, string userId);
        public Task<ItemReadDTO> CreateItemAsync(string userId, ItemWriteDTO itemWriteDTO);
        //public Task<ItemReadDTO> CreateItemAsync(ItemValue itemValue, string categoryName, string userId, ItemWriteDTO itemWriteDTO);
        public Task<ItemReadDTO> UpdateItemStatus(int itemId, ItemStatus itemStatus);
        public Task<ItemReadDTO> UpdateItemDetailsWithoutCabinetIdAsync(int itemId, ItemUpdateWithoutCabinetIdDTO itemUpdateDTO);
        public Task<IEnumerable<ItemReadWithClaimStatusDTO>> GetClaimsForMember(string userId);
        public Task<IEnumerable<ItemReadWithClaimStatusDTO>> GetAllClaimsForManager();
        public Task<ItemReadWithClaimStatusDTO> GetAnItemWithClaimsForFounder(string userId, int itemId);
        public Task<ItemReadWithClaimStatusDTO> GetAnItemWithClaimsForMember(string userId, int itemId);
        public Task<ItemReadWithClaimStatusDTO> GetAnItemWithClaimsForManager(int itemId);
        public Task<ItemReadDTO> UpdateItemCabinet(int itemId, int cabinetId);
        public Task<IEnumerable<ItemReadDTO>> ListItemsSortByFloorNumberAsync();
        public Task UpdateClaimStatusAsync(int itemId, string userId);
        public Task AcceptAClaimAsync(int itemId, string userId);
        public Task<TransferRecordReadDTO> AcceptAClaimAndCreateReceiptAsync(int itemId, string receiverId, IFormFile receiptMedia);
        public Task DenyAClaimAsync(int itemId, string userId);
        public Task<ItemReadWithReceiptDTO> ReceiveAnItemIntoStorageAsync(string userId, ItemIntoStorageWithReceiptWriteDTO writeDTO);
        public Task<ItemReadWithReceiptDTO> TransferAnItemIntoStorageAsync(string userId, ItemUpdateTransferToStorageDTO updateDTO);
    }
}
