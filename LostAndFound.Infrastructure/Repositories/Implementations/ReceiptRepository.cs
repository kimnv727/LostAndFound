using Amazon.Runtime;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.DTOs.Receipt;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class ReceiptRepository : GenericRepository<Receipt>, IReceiptRepository
    {
        public ReceiptRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Receipt>> QueryReceiptAsync(ReceiptQuery query, bool trackChanges = false)
        {
            IQueryable<Receipt> receipts = _context.Receipts
                .Include(r => r.Media)
                .AsSplitQuery();

            if (!trackChanges)
            {
                receipts = receipts.AsNoTracking();
            }

            if (query.Id > 0)
            {
                receipts = receipts.Where(r => r.Id == query.Id);
            }

            if (!string.IsNullOrWhiteSpace(query.ReceiverId))
            {
                receipts = receipts.Where(r => r.ReceiverId.ToLower().Trim().Contains(query.ReceiverId.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(query.SenderId))
            {
                receipts = receipts.Where(r => r.SenderId.ToLower().Trim().Contains(query.SenderId.ToLower()));
            }

            if (query.ItemId > 0)
            {
                receipts = receipts.Where(r => r.ItemId == query.ItemId);
            }

            if (Enum.IsDefined(query.ReceiptType))
            {
                switch (query.ReceiptType)
                {
                    case ReceiptType.OUTSTORAGE:
                        receipts = receipts.Where(r => r.ReceiptType == ReceiptType.OUTSTORAGE);
                        break;
                    case ReceiptType.INSTORGE:
                        receipts = receipts.Where(r => r.ReceiptType == ReceiptType.INSTORGE);
                        break;
                    case ReceiptType.GIVEAWAY:
                        receipts = receipts.Where(r => r.ReceiptType == ReceiptType.GIVEAWAY);
                        break;
                }
            }

            if (query.ReceiptImage != null)
            {
                receipts = receipts.Where(r => r.ReceiptImage == query.ReceiptImage);
            }

            if (query.CreatedDate > DateTime.MinValue)
            {
                receipts = receipts.Where(r => r.CreatedDate == query.CreatedDate).OrderBy(r => r.CreatedDate);
            }

            return await Task.FromResult(receipts.ToList());
        }

        public async Task<IEnumerable<Receipt>> GetAllWithMediaAsync()
        {
            return await  _context.Receipts
                .Include(r => r.Media)
                .ToListAsync();
        }

        public Task<Receipt> GetReceiptByIdAsync(int receiptId)
        {
            return _context.Receipts
                .Include(r => r.Media)
                .FirstOrDefaultAsync(r => r.Id == receiptId);
        }
    }
}
