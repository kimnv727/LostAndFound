using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Category;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<PaginatedResponse<CategoryReadDTO>> QueryCategoryAsync(CategoryQuery query);
        public Task<CategoryReadDTO> ChangeCategoryStatusAsync(int id);
        public Task<IEnumerable<CategoryReadDTO>> ListAllWithGroupAsync();
        public Task<IEnumerable<CategoryReadDTO>> ListAllByGroupIdAsync(int categoryGroupId);
        public Task<CategoryReadDTO> FindCategoryByIdAsync(int categoryId);
        /*public Task<CategoryReadDTO> FindCategoryByNameAsync(string categoryName);*/
        public Task DeleteCategoryAsync(int categoryId);
        public Task<CategoryReadDTO> UpdateCategoryAsync(int categoryId, CategoryWriteDTO categoryWriteDTO);
        public Task<CategoryReadDTO> CreateCategoryAsync(string userId, CategoryWriteDTO categoryWriteDTO);
    }
}