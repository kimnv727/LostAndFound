using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.CategoryGroup;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class CategoryGroupRepository : GenericRepository<CategoryGroup>, ICategoryGroupRepository
    {
        public CategoryGroupRepository(LostAndFoundDbContext context) : base(context)
        {
        }
        
        public async Task<IEnumerable<CategoryGroup>> QueryCategoryGroupAsync(CategoryGroupQuery query, bool trackChanges = false)
        {
            IQueryable<CategoryGroup> categoryGroups = _context.CategoryGroups.AsSplitQuery();

            if (!trackChanges)
            {
                categoryGroups = categoryGroups.AsNoTracking();
            }
            
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                categoryGroups = categoryGroups.Where(cg => cg.Name.ToLower().Contains(query.Name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                categoryGroups = categoryGroups.Where(cg => cg.Description.ToLower().Contains(query.Description.ToLower()));
            }

            if (Enum.IsDefined(query.Value))
            {
                switch (query.Value)
                {
                    case ItemValue.High:
                        categoryGroups = categoryGroups.Where(cg => cg.Value == ItemValue.High);
                        break;
                    case ItemValue.Low:
                        categoryGroups = categoryGroups.Where(cg => cg.Value == ItemValue.Low);
                        break;
                }
            }
            
                
            return await Task.FromResult(categoryGroups.ToList());
        }

        public async Task<CategoryGroup> FindCategoryGroupByIdAsync(int categoryGroupId)
        {
            return await _context.CategoryGroups.FirstOrDefaultAsync(cg => cg.Id == categoryGroupId);
        }

        public async Task<CategoryGroup> FindCategoryGroupByNameAsync(string categoryGroupName)
        {
            return await _context.CategoryGroups.FirstOrDefaultAsync(cg => cg.Name.ToLower().Contains(categoryGroupName.ToLower()));
        }
    }
}