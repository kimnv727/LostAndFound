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
                .Where(ic => ic.UserId == userId)
                .OrderByDescending(i => i.ClaimDate)
                .ToList());
        }

        public async Task<IEnumerable<ItemClaim>> GetAllClaimsByItemIdAsync(int itemId)
        {
            return await Task.FromResult(
                _context.ItemClaims
                .AsSplitQuery()
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
                if (query.ClaimStatus >= 0)
                {
                    switch (query.ClaimStatus)
                    {
                        default:
                            break;
                        case 0:
                            claims = claims.Where(ic => ic.ClaimStatus == 0);
                            break;
                        case 1:
                            claims = claims.Where(ic => ic.ClaimStatus == 1);
                            break;
                    }
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
