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
        IGetAllAsync<Receipt>,
        IAddAsync<Receipt>,
        IUpdate<Receipt>,
        IDelete<Receipt>
    {
        public Task<IEnumerable<Receipt>> QueryReceiptAsync(ReceiptQuery query, bool trackChanges = false);
        public Task<Receipt> GetReceiptByIdAsync(int receiptId);
        public Task<IEnumerable<Receipt>> GetAllWithMediaAsync();
    }
}
