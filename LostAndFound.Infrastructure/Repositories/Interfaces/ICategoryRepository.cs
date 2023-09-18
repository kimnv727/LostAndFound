using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Category;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface ICategoryRepository : 
        IGetAllAsync<Category>,
        IDelete<Category>,
        IUpdate<Category>,
        IFindAsync<Category>,
        IAddAsync<Category>
    {
        public Task<Category> FindCategoryByIdAsync(int categoryId);
        
        public Task<Category> FindCategoryByNameAsync(string categoryName);

        public Task<IEnumerable<Category>> QueryCategoriesAsync(CategoryQuery query, bool trackChanges = false);

        public Task<Category> GetCategoryWithCategoryGroup(int categoryId);
    }
}