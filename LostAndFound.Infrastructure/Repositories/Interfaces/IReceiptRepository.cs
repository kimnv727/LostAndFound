using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Receipt;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IReceiptRepository :
        IGetAllAsync<TransferRecord>,
        IAddAsync<TransferRecord>,
        IUpdate<TransferRecord>,
        IDelete<TransferRecord>
    {
        public Task<IEnumerable<TransferRecord>> QueryReceiptAsync(TransferRecordQuery query, bool trackChanges = false);
        public Task<TransferRecord> GetReceiptByIdAsync(int receiptId);
        public Task<IEnumerable<TransferRecord>> GetAllWithMediaAsync();
        public Task<IEnumerable<TransferRecord>> GetAllWithItemIdAsync(int itemId);
        public Task<IEnumerable<TransferRecord>> GetLatestTenOfAMonthAsync(int month, int year);
    }
}
