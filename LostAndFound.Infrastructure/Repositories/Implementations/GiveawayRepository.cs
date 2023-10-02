using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Giveaway;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class GiveawayRepository : GenericRepository<Giveaway>, IGiveawayRepository
    {
        public GiveawayRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<Giveaway> FindGiveawayByIdAsync(int id)
        {
            return await _context.Giveaways
                .Include(g => g.Item)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Giveaway> FindGiveawayIncludeParticipantssAsync(int id)
        {
            return await _context.Giveaways
                .Include(g => g.Item)
                .Include(g => g.GiveawayParticipants)
                .ThenInclude(gp => gp.User)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Giveaway>> QueryGiveawayAsync(GiveawayQuery query, bool trackChanges = false)
        {
            IQueryable<Giveaway> giveaways = _context.Giveaways
                .Include(g => g.Item)
                .Where(g => g.GiveawayStatus == GiveawayStatus.ONGOING).AsSplitQuery();

            if (!trackChanges)
            {
                giveaways = giveaways.AsNoTracking();
            }

            if (query.ItemCategoryGroupId > 0)
            {
                giveaways = giveaways.Where(g => g.Item.Category.CategoryGroupId == query.ItemCategoryGroupId);
            }
            
            if (query.ItemCategoryId > 0)
            {
                giveaways = giveaways.Where(g => g.Item.CategoryId == query.ItemCategoryId);
            }

            if (query.StartAt != null)
            {
                giveaways = giveaways.Where(g => g.StartAt >= query.StartAt);
            }
            
            if (query.EndAt != null)
            {
                giveaways = giveaways.Where(g => g.EndAt <= query.EndAt);
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                giveaways = giveaways.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(giveaways.ToList());
        }
        
        public async Task<IEnumerable<Giveaway>> QueryGiveawayWithStatusAsync(GiveawayQueryWithStatus query, bool trackChanges = false)
        {
            IQueryable<Giveaway> giveaways = _context.Giveaways
                .Include(g => g.Item)
                .AsSplitQuery();

            if (!trackChanges)
            {
                giveaways = giveaways.AsNoTracking();
            }

            if (query.ItemCategoryGroupId > 0)
            {
                giveaways = giveaways.Where(g => g.Item.Category.CategoryGroupId == query.ItemCategoryGroupId);
            }
            
            if (query.ItemCategoryId > 0)
            {
                giveaways = giveaways.Where(g => g.Item.CategoryId == query.ItemCategoryId);
            }

            if (query.StartAt != null)
            {
                giveaways = giveaways.Where(g => g.StartAt >= query.StartAt);
            }
            
            if (query.EndAt != null)
            {
                giveaways = giveaways.Where(g => g.EndAt <= query.EndAt);
            }

            if (Enum.IsDefined(query.GiveawayStatus))
            {
                if (query.GiveawayStatus == GiveawayQueryWithStatus.GiveawayStatusQuery.NOTSTARTED)
                {
                    giveaways = giveaways.Where(g => g.GiveawayStatus == GiveawayStatus.NOTSTARTED);
                }
                else if (query.GiveawayStatus == GiveawayQueryWithStatus.GiveawayStatusQuery.ONGOING)
                {
                    giveaways = giveaways.Where(g => g.GiveawayStatus == GiveawayStatus.ONGOING);
                }
                else if (query.GiveawayStatus == GiveawayQueryWithStatus.GiveawayStatusQuery.CLOSED)
                {
                    giveaways = giveaways.Where(g => g.GiveawayStatus == GiveawayStatus.CLOSED);
                }
                else if (query.GiveawayStatus == GiveawayQueryWithStatus.GiveawayStatusQuery.WAITINGRESULT)
                {
                    giveaways = giveaways.Where(g => g.GiveawayStatus == GiveawayStatus.WAITINGRESULT);
                }
            }
            
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                giveaways = giveaways.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(giveaways.ToList());
        }
    }
}