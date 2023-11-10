using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.CategoryGroup;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface ICategoryGroupService 
    {
        public Task<PaginatedResponse<CategoryGroupReadDTO>> QueryCategoryGroupAsync(CategoryGroupQuery query);

        public Task<IEnumerable<CategoryGroupReadDTO>> ListAllWithCategoriesAsync();

        public Task<CategoryGroupReadDTO> ChangeCategoryGroupStatusAsync(int id);

        public Task<CategoryGroupReadDTO> GetCategoryGroupByIdAsync(int categoryGroupId);

        Task<CategoryGroupReadDTO> CreateCategoryGroupAsync(string userId, CategoryGroupWriteDTO categoryGroupWriteDTO);

        public Task<CategoryGroupReadDTO> UpdateCategoryGroupDetailsAsync(int categoryGroupId, CategoryGroupWriteDTO categoryGroupWriteDTO);
        
        public Task DeleteCategoryGroupAsync(int categoryGroupId);
    }
}