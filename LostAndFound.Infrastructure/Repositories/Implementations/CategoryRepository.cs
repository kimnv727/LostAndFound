using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
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
        public Task<Category> FindCategoryByIdAsync(int CategoryId)
        {
            return _context.Categories.FirstOrDefaultAsync(i => i.Id == CategoryId);
        }
        
        public Task<Category> GetCategoryWithCategoryGroup(int categoryId)
        {
            return _context.Categories
                .Include(c => c.CategoryGroupId) 
                .FirstOrDefaultAsync(c => c.Id == categoryId);
        }


        public Task<Category> FindCategoryByNameAsync(string Name)
        {
            return _context.Categories.FirstOrDefaultAsync
                (i => i.Name.ToLower().Contains(Name.ToLower()));
        }

        public async Task<IEnumerable<Category>> QueryCategoriesAsync(CategoryQuery query, bool trackChanges = false)
        {
            
            IQueryable<Category> categories = _context.Categories.AsSplitQuery();

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

            return await Task.FromResult(categories.ToList());
        }

        public async Task<IEnumerable<Category>> QueryCategoryIgnoreStatusAsync(CategoryQuery query, bool trackChanges = false)
        {
            IQueryable<Category> Categories = _context.Categories.AsSplitQuery();

            if (!trackChanges)
            {
                Categories = Categories.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                Categories = Categories.Where(i => i.Name.ToLower().Contains(query.Name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                Categories = Categories.Where(i => i.Description.ToLower().Contains(query.Description.ToLower()));
            }

            return await Task.FromResult(Categories.ToList());
        }
    }
}