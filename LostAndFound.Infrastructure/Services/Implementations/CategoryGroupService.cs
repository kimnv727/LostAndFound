using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.CategoryGroup;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.CategoryGroup;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class CategoryGroupService : ICategoryGroupService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryGroupRepository _categoryGroupRepository;
        private readonly IUserRepository _userRepository;

        public CategoryGroupService(IMapper mapper, IUnitOfWork unitOfWork, ICategoryGroupRepository categoryGroupRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _categoryGroupRepository = categoryGroupRepository;
            _userRepository = userRepository;
        }
        
        public async Task<PaginatedResponse<CategoryGroupReadDTO>> QueryCategoryGroupAsync(CategoryGroupQuery query)
        {
            var categoryGroups = await _categoryGroupRepository.QueryCategoryGroupAsync(query);
            return PaginatedResponse<CategoryGroupReadDTO>.FromEnumerableWithMapping(categoryGroups, query, _mapper);
        }

        public async Task<CategoryGroupReadDTO> GetCategoryGroupByIdAsync(int categoryGroupId)
        {
            var categoryGroup = await _categoryGroupRepository.FindCategoryGroupByIdAsync(categoryGroupId);
            if (categoryGroup == null)
            {
                throw new EntityWithIDNotFoundException<CategoryGroup>(categoryGroupId);
            }
            
            return _mapper.Map<CategoryGroupReadDTO>(categoryGroup);
        }

        public async Task<CategoryGroupReadDTO> CreateCategoryGroupAsync(string userId, CategoryGroupWriteDTO categoryGroupWriteDTO)
        {
            //Check if user exist
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            
            //Check duplicate name
            var category = await _categoryGroupRepository.FindCategoryGroupByNameAsync(categoryGroupWriteDTO.Name);
            if (category != null)
            {
                throw new CategoryGroupNameAlreadyUsedException();
            }

            var categoryGroup = _mapper.Map<CategoryGroup>(categoryGroupWriteDTO);
            
            await _categoryGroupRepository.AddAsync(categoryGroup);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CategoryGroupReadDTO>(categoryGroup);
        }
        
        public async Task<CategoryGroupReadDTO> UpdateCategoryGroupDetailsAsync(int categoryGroupId, CategoryGroupWriteDTO categoryGroupWriteDTO)
        {
            var categoryGroup = await _categoryGroupRepository.FindCategoryGroupByIdAsync(categoryGroupId);

            if (categoryGroup == null)
            {
                throw new EntityWithIDNotFoundException<CategoryGroup>(categoryGroupId);
            }

            _mapper.Map(categoryGroupWriteDTO, categoryGroup);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CategoryGroupReadDTO>(categoryGroup);
        }
        
        public async Task DeleteCategoryGroupAsync(int categoryGroupId)
        {
            var categoryGroup = await _categoryGroupRepository.FindCategoryGroupByIdAsync(categoryGroupId);

            if (categoryGroup == null)
            {
                throw new EntityWithIDNotFoundException<CategoryGroup>(categoryGroupId);
            }

            _categoryGroupRepository.Delete(categoryGroup);
            await _unitOfWork.CommitAsync();
        }
    }
}