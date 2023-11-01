using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Category;
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
        private readonly IUserRepository _userRepository;

        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        public async Task<PaginatedResponse<CategoryReadDTO>> QueryCategoryAsync(CategoryQuery query)
        {
            var categories = await _categoryRepository.QueryCategoriesAsync(query);

            return PaginatedResponse<CategoryReadDTO>.FromEnumerableWithMapping(categories, query, _mapper);
        }

        public async Task<IEnumerable<CategoryReadDTO>> ListAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return _mapper.Map<List<CategoryReadDTO>>(categories);
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

        /*public async Task<CategoryReadDTO> FindCategoryByNameAsync(string categoryName)
        {
            var category = await _categoryRepository.FindCategoryByNameAsync(categoryName);

            if (category == null)
            {
                throw new EntityWithAttributeNotFoundException<Category>("name ", categoryName);
            }

            return _mapper.Map<CategoryReadDTO>(category);
        }*/
        
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

        public async Task<CategoryReadDTO> ChangeCategoryStatusAsync(int id)
        {
            var category = await _categoryRepository.FindCategoryByIdAsync(id);
            if (category == null)
            {
                throw new EntityWithIDNotFoundException<Category>(id);
            }
            if(category.IsActive == true)
            {
                foreach(var item in category.Items)
                {
                    if(item.ItemStatus == Core.Enums.ItemStatus.ACTIVE || 
                        item.ItemStatus == Core.Enums.ItemStatus.PENDING || 
                        item.ItemStatus == Core.Enums.ItemStatus.REJECTED)
                    {
                        throw new CategoryStillHaveItemOrPostException();
                    }
                }

                foreach (var post in category.Posts)
                {
                    if (post.PostStatus == Core.Enums.PostStatus.ACTIVE ||
                        post.PostStatus == Core.Enums.PostStatus.PENDING ||
                        post.PostStatus == Core.Enums.PostStatus.REJECTED)
                    {
                        throw new CategoryStillHaveItemOrPostException();
                    }
                }
            }

            category.IsActive = !category.IsActive;
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CategoryReadDTO>(category);
        }

        public async Task<CategoryReadDTO> CreateCategoryAsync(string userId, CategoryWriteDTO categoryWriteDTO)
        {
            //Check if user exist
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            
            //Check name
            var category = await _categoryRepository.FindCategoryByNameAsync(categoryWriteDTO.Name);

            if (category != null)
            {
                throw new CategoryNameAlreadyUsedException();
            }

            category = _mapper.Map<Category>(categoryWriteDTO);
            await _categoryRepository.AddAsync(category);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CategoryReadDTO>(category);
        }
    }
}