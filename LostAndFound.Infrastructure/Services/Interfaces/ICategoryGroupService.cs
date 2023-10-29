using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.CategoryGroup;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface ICategoryGroupService 
    {
        Task<PaginatedResponse<CategoryGroupReadDTO>> QueryCategoryGroupAsync(CategoryGroupQuery query);

        Task<IEnumerable<CategoryGroupReadDTO>> ListAllAsync();

        Task<CategoryGroupReadDTO> GetCategoryGroupByIdAsync(int categoryGroupId);

        Task<CategoryGroupReadDTO> CreateCategoryGroupAsync(string userId, CategoryGroupWriteDTO categoryGroupWriteDTO);

        Task<CategoryGroupReadDTO> UpdateCategoryGroupDetailsAsync(int categoryGroupId, CategoryGroupWriteDTO categoryGroupWriteDTO);
        
        Task DeleteCategoryGroupAsync(int categoryGroupId);
    }
}