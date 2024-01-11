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
using static LostAndFound.Infrastructure.DTOs.Receipt.TransferRecordQuery;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class ReceiptRepository : GenericRepository<TransferRecord>, IReceiptRepository
    {
        public ReceiptRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TransferRecord>> QueryReceiptAsync(TransferRecordQuery query, bool trackChanges = false)
        {
            IQueryable<TransferRecord> receipts = _context.TransferRecords
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemClaims.Where(ic => ic.ClaimStatus == ClaimStatus.ACCEPTED))
                        .ThenInclude(ic => ic.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                        .ThenInclude(im => im.Media)
                .Include(r => r.Item)
                    .ThenInclude(i => i.User)
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
                    case ReceiptTypeQuery.IN_STORAGE:
                        receipts = receipts.Where(r => r.ReceiptType == ReceiptType.IN_STORAGE);
                        break;
                    case ReceiptTypeQuery.RETURN_OUT_STORAGE:
                        receipts = receipts.Where(r => r.ReceiptType == ReceiptType.RETURN_OUT_STORAGE);
                        break;
                    case ReceiptTypeQuery.RETURN_USER_TO_USER:
                        receipts = receipts.Where(r => r.ReceiptType == ReceiptType.RETURN_USER_TO_USER);
                        break;
                    case ReceiptTypeQuery.GIVEAWAY_OUT_STORAGE:
                        receipts = receipts.Where(r => r.ReceiptType == ReceiptType.GIVEAWAY_OUT_STORAGE);
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

        public async Task<IEnumerable<TransferRecord>> GetAllWithMediaAsync()
        {
            return await  _context.TransferRecords
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemClaims.Where(ic => ic.ClaimStatus == ClaimStatus.ACCEPTED))
                        .ThenInclude(ic => ic.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                        .ThenInclude(im => im.Media)
                .Include(r => r.Item)
                    .ThenInclude(i => i.User)
                .Include(r => r.Media)
                .ToListAsync();
        }

        public async Task<TransferRecord> GetReceiptByIdAsync(int receiptId)
        {
            return await _context.TransferRecords
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemClaims.Where(ic => ic.ClaimStatus == ClaimStatus.ACCEPTED))
                        .ThenInclude(ic => ic.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                        .ThenInclude(im => im.Media)
                .Include(r => r.Item)
                    .ThenInclude(i => i.User)
                .Include(r => r.Media)
                .FirstOrDefaultAsync(r => r.Id == receiptId);
        }

        public async Task<IEnumerable<TransferRecord>> GetReceiptsByUserIdAsync(string userId)
        {
            return await _context.TransferRecords
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemClaims.Where(ic => ic.ClaimStatus == ClaimStatus.ACCEPTED))
                        .ThenInclude(ic => ic.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                        .ThenInclude(im => im.Media)
                .Include(r => r.Item)
                    .ThenInclude(i => i.User)
                .Include(r => r.Media)
                .Where(r => (r.ReceiverId ==  userId || r.SenderId == userId))
                .ToListAsync();
        }

        public async Task<IEnumerable<TransferRecord>> GetAllWithItemIdAsync(int itemId)
        {

            IQueryable<TransferRecord> receipts = _context.TransferRecords
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemClaims.Where(ic => ic.ClaimStatus == ClaimStatus.ACCEPTED))
                        .ThenInclude(ic => ic.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                        .ThenInclude(im => im.Media)
                .Include(r => r.Item)
                    .ThenInclude(i => i.User)
                .Include(r => r.Media)
                .Where(r => r.ItemId == itemId).OrderBy(r => r.CreatedDate);

            return await Task.FromResult(receipts.ToList());
        }

        public async Task<IEnumerable<TransferRecord>> GetLatestTenOfAMonthAsync(int month, int year)
        {

            IQueryable<TransferRecord> receipts = _context.TransferRecords
                .Include(r => r.Item)
                    .ThenInclude(i => i.Category)
                        .ThenInclude(c => c.CategoryGroup)
                .Include(r => r.Item)
                    .ThenInclude(i => i.Location)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemClaims.Where(ic => ic.ClaimStatus == ClaimStatus.ACCEPTED))
                        .ThenInclude(ic => ic.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                        .ThenInclude(im => im.Media)
                .Include(r => r.Item)
                    .ThenInclude(i => i.User)
                .Include(r => r.Media)
                .Where(r => r.CreatedDate.Month == month && r.CreatedDate.Year == year 
                && (r.ReceiptType == ReceiptType.RETURN_OUT_STORAGE || r.ReceiptType == ReceiptType.RETURN_USER_TO_USER))
                .OrderByDescending(r => r.CreatedDate)
                .Take(10);

            return await Task.FromResult(receipts.ToList());
        }
    }
}
