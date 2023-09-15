using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.Category;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        
        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        public async Task<PaginatedResponse<CategoryReadDTO>> QueryCategoryAsync(CategoryQuery query)
        {
            var categories = await _categoryRepository.QueryCategoriesAsync(query);

            return PaginatedResponse<CategoryReadDTO>.FromEnumerableWithMapping(categories, query, _mapper);
        }

        public async Task<CategoryReadDTO> FindCategoryByIdAsync(int categoryId)
        {
            var category = await _categoryRepository.FindCategoryByIdAsync(categoryId);

            if (category == null)
            {
                throw new EntityWithIDNotFoundException<Category>(categoryId);
            }
            
            return _mapper.Map<CategoryReadDTO>(category);
        }
        
        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.FindCategoryByIdAsync(categoryId);

            if (category == null)
            {
                throw new EntityWithIDNotFoundException<Category>(categoryId);
            }
            
            _categoryRepository.Delete(category);
            await _unitOfWork.CommitAsync();
        }

        public async Task<CategoryReadDTO> UpdateCategoryAsync(int categoryId, CategoryWriteDTO categoryWriteDTO)
        {
            var category = await _categoryRepository.FindCategoryByIdAsync(categoryId);

            if (category == null)
            {
                throw new EntityWithIDNotFoundException<Category>(categoryId);
            }

            _mapper.Map(categoryWriteDTO, category);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CategoryReadDTO>(category);
        }
    }
}