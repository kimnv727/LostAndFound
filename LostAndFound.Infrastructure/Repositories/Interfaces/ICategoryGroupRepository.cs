using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.CategoryGroup;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface ICategoryGroupRepository :
        IAddAsync<CategoryGroup>,
        IDelete<CategoryGroup>,
        IGetAllAsync<CategoryGroup>,
        IUpdate<CategoryGroup>,
        IFindAsync<CategoryGroup>
    {
        public Task<IEnumerable<CategoryGroup>> QueryCategoryGroupAsync(CategoryGroupQuery query,
            bool trackChanges = false);

        public Task<CategoryGroup> FindCategoryGroupByIdAsync(int categoryGroupId);
        public Task<CategoryGroup> FindCategoryGroupByNameAsync(string categoryGroupName);
    }
}