using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    internal class ItemRepository : GenericRepository<Item>, IITemRepository
    {
        public ItemRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public Task<Item> FindItemByIdAsync(Guid itemId)
        {

            return null;
        }

        public async Task<IEnumerable<Item>> QueryItemAsync(ItemQuery query, bool trackChanges = false)
        {
            IQueryable<Item> items = _context.Items.Where(i => i.IsActive == true).AsSplitQuery();

            if (!trackChanges)
            {
                items = items.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                items = items.Where(i => i.Name.ToLower().Contains(query.Name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                items = items.Where(i => i.Description.ToLower().Contains(query.Description.ToLower()));
            }

            return await Task.FromResult(items.ToList());
        }
    }
}
