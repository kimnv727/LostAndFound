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

    }
}