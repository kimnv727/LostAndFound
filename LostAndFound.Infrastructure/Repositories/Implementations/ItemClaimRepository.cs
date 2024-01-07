using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.ItemClaim;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class ItemClaimRepository : GenericRepository<ItemClaim>, IItemClaimRepository
    {
        public ItemClaimRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ItemClaim>> GetAllClaimsByUserIdAsync(string userId)
        {
            return await Task.FromResult(
                _context.ItemClaims
                .AsSplitQuery()
                .Include(ic => ic.User)
                .Include(ic => ic.Item)
                .Where(ic => ic.UserId == userId)
                .OrderByDescending(i => i.ClaimDate)
                .ToList());
        }

        public async Task<IEnumerable<ItemClaim>> GetAllActiveClaimsByUserIdAsync(string userId)
        {
            return await Task.FromResult(
                _context.ItemClaims
                .AsSplitQuery()
                .Include(ic => ic.User)
                .Include(ic => ic.Item)
                .Where(ic => ic.UserId == userId && ic.IsActive == true && ic.Item.ItemStatus == ItemStatus.ACTIVE)
                .OrderByDescending(i => i.ClaimDate)
                .ToList());
        }

        public async Task<IEnumerable<ItemClaim>> GetAllActiveClaimsByItemIdAsync(int itemId)
        {
            var claims = _context.ItemClaims
                .AsSplitQuery()
                .Where(ic => ic.IsActive == true)
                .Include(ic => ic.User)
                .Include(ic => ic.Item)
                .Where(ic => ic.ItemId == itemId)
                .OrderByDescending(i => i.ClaimDate);

            return await Task.FromResult(claims.ToList());
        }

        public async Task<IEnumerable<ItemClaim>> GetAllClaimsByItemIdAsync(int itemId)
        {
            return await Task.FromResult(
                _context.ItemClaims
                .AsSplitQuery()
                .Include(ic => ic.User)
                .Include(ic => ic.Item)
                .Where(ic => ic.ItemId == itemId)
                .OrderByDescending(i => i.ClaimDate)
                .ToList());
        }

        public async Task<IEnumerable<ItemClaim>> GetItemClaimsByClaimStatus(int itemId)
        {
            return await Task.FromResult(
                _context.ItemClaims
                .AsSplitQuery()
                .Include(ic => ic.User)
                .Include(ic => ic.Item)
                .Where(ic => ic.ItemId == itemId)
                .OrderByDescending(i => i.ClaimDate)
                .ToList());
        }



        public Task<ItemClaim> FindClaimByItemIdAndUserId(int itemId, string userId)
        {
            return Task.FromResult(
                _context.ItemClaims
                .FirstOrDefault(ic => ic.ItemId == itemId && ic.UserId == userId));
        }

        public async Task<IEnumerable<ItemClaim>> QueryItemClaimsAsync(ItemClaimQuery query, bool trackChanges = false)
        {
            IQueryable<ItemClaim> claims = _context.ItemClaims
                                        .Include(i => i.User)
                                        .Include(i => i.Item)
                                        .AsSplitQuery();
            //Order by claim date descending
            claims = claims.OrderByDescending(ic => ic.ClaimDate);

            if (!trackChanges)
            {
                if (query.ItemId > 0)
                {
                    claims = claims.Where(ic => ic.ItemId == query.ItemId);
                }
                if (!string.IsNullOrEmpty(query.UserId))
                {
                    claims = claims.Where(ic => ic.UserId == query.UserId);
                }
                if (Enum.IsDefined(query.ClaimStatus))
                {
                    switch (query.ClaimStatus)
                    {

                        case ItemClaimQuery.ClaimStatusQuery.ALL:
                            break;
                        case ItemClaimQuery.ClaimStatusQuery.PENDING:
                            claims = claims.Where(ic => ic.ClaimStatus == ClaimStatus.PENDING);
                            break;
                        case ItemClaimQuery.ClaimStatusQuery.ACCEPTED:
                            claims = claims.Where(ic => ic.ClaimStatus == ClaimStatus.ACCEPTED);
                            break;
                        case ItemClaimQuery.ClaimStatusQuery.DENIED:
                            claims = claims.Where(ic => ic.ClaimStatus == ClaimStatus.DENIED);
                            break;
                        default:
                            break;
                    }
                }
                if (query.IsActive == true)
                {
                    claims = query.IsActive == true ? claims.Where(ic => ic.IsActive == true) 
                                                       : claims.Where(ic => ic.IsActive == false);
                }
                if (query.ClaimDate > DateTime.MinValue)
                {
                    claims = claims.Where(ic => ic.ClaimDate == query.ClaimDate);
                }
            }

            return await Task.FromResult(claims.ToList());
        }
    }
}
