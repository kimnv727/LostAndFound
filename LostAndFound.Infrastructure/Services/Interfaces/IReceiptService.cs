using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.DTOs.Receipt;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IReceiptService
    {
        public Task<PaginatedResponse<TransferRecordReadDTO>> QueryReceiptAsync(TransferRecordQuery query);
        public Task<TransferRecordReadDTO> CreateReceiptAsync(TransferRecordCreateDTO receiptCreateDTO, IFormFile image);
        public Task<TransferRecordReadDTO> CreateReceiptForGiveawayAsync(string currentUserId, TransferRecordGiveawayCreateDTO receiptCreateDTO, IFormFile image);
        public Task DeleteReceiptAsync(int receiptId);
        public Task<IEnumerable<TransferRecordReadDTO>> ListAllAsync();
        public Task<TransferRecordReadDTO> FindReceiptByIdAsync(int receiptId);
        public Task<IEnumerable<TransferRecordReadDTO>> GetAllReceiptsByItemIdAsync(int itemId);
        public Task<TransferRecordReadDTO> RevokeReceipt(int receiptId);
        public Task<TransferRecordReadDTO> CreateReceiptForOnHoldItemAsync(string currentUserId, TransferRecordOnholdItemCreateDTO receiptCreateDTO, IFormFile image);
        public Task<IEnumerable<TransferRecordReadWithUserDTO>> GetReceiptsByUserIdAsync(string userId);
    }
}
