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
        public Task<PaginatedResponse<ReceiptReadDTO>> QueryReceiptAsync(ReceiptQuery query);
        public Task<ReceiptReadDTO> CreateReceiptAsync(ReceiptCreateDTO receiptCreateDTO, IFormFile image);
        public Task<MediaReadDTO> UploadReceiptImageAsync(int itemId, IFormFile image);
        public Task DeleteReceiptAsync(int receiptId);
        public Task<IEnumerable<ReceiptReadDTO>> ListAllAsync();
        public Task<ReceiptReadDTO> GetReceiptByIdAsync(int receiptId);
    }
}
