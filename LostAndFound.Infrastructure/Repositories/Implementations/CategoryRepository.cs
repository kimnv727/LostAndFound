using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Category;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(LostAndFoundDbContext context) : base(context)
        {
        }
        public Task<Category> FindCategoryByIdAsync(int categoryId)
        {
            return _context.Categories
                .Include(c => c.CategoryGroup)
                .Include(c => c.Items)
                //.Include(c => c.Posts)
                .FirstOrDefaultAsync(i => i.Id == categoryId);
        }
        
        public Task<Category> FindCategoryByNameAsync(string categoryName)
        {
            return _context.Categories
                .Include(c => c.CategoryGroup)
                .Include(c => c.Items)
                //.Include(c => c.Posts)
                .FirstOrDefaultAsync
                (i => i.Name.ToLower() == (categoryName.ToLower()));
        }
        
        public Task<Category> GetCategoryWithCategoryGroup(int categoryId)
        {
            return _context.Categories
                .Include(c => c.CategoryGroupId) 
                .FirstOrDefaultAsync(c => c.Id == categoryId);
        }
        
        public async Task<IEnumerable<Category>> GetAllWithGroupsAsync()
        {
            var result = _context
                                            .Categories
                                            .Include(c => c.CategoryGroup)
                                            .AsSplitQuery();
            result = result.AsNoTracking();

            return await Task.FromResult(result.ToList());
        }

        public async Task<IEnumerable<Category>> QueryCategoriesAsync(CategoryQuery query, bool trackChanges = false)
        {
            
            IQueryable<Category> categories = _context.Categories
                .Include(c => c.CategoryGroup)
                .AsSplitQuery();

            /*List<Category> categories2 = new List<Category>();
            foreach(Category category in categories)
            {
                var category2 = GetCategoryWithCategoryGroup(category.Id);
                categories2.Add(category2);
            }*/
            
            if (!trackChanges)
            {
                categories = categories.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                categories = categories.Where(i => i.Name.ToLower().Contains(query.Name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                categories = categories.Where(i => i.Description.ToLower().Contains(query.Description.ToLower()));
            }
            if (Enum.IsDefined(query.IsSensitive))
            {
                switch (query.IsSensitive)
                {
                    case CategoryQuery.SensitiveStatusSearch.True:
                        categories = categories.Where(c => c.IsSensitive == true);
                        break;
                    case CategoryQuery.SensitiveStatusSearch.False:
                        categories = categories.Where(c => c.IsSensitive == false);
                        break;
                }
            }
            if (Enum.IsDefined(query.IsActive))
            {
                switch (query.IsActive)
                {
                    case CategoryQuery.ActiveStatus.Active:
                        categories = categories.Where(c => c.IsActive == true);
                        break;
                    case CategoryQuery.ActiveStatus.Disabled:
                        categories = categories.Where(c => c.IsActive == false);
                        break;
                }
            }
            if (Enum.IsDefined(query.Value))
            {
                switch (query.Value)
                {
                    case CategoryQuery.ItemValueSearch.High:
                        categories = categories.Where(c => c.Value == ItemValue.High);
                        break;
                    case CategoryQuery.ItemValueSearch.Low:
                        categories = categories.Where(c => c.Value == ItemValue.Low);
                        break;
                }
            }

            return await Task.FromResult(categories.ToList());
        }

        public async Task<IEnumerable<Category>> GetAllWithGroupsByIdArrayAsync(int[] categoryIds)
        {
            var result = _context.Categories
                .Where(c => categoryIds.Contains(c.Id));

            return await Task.FromResult(result.ToList());
        }

        public async Task<IEnumerable<Category>> GetAllByGroupIdAsync(int categroupId)
        {
            var result = _context.Categories
                .Where(c => c.CategoryGroupId == categroupId && c.IsActive == true)
                .AsSplitQuery();
            result = result.AsNoTracking();

            return await Task.FromResult(result.ToList());
        }
    }
}