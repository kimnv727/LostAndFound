using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.Property;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class PropertyService : IPropertyService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUserRepository _userRepository;
        
        public PropertyService(IMapper mapper, IUnitOfWork unitOfWork, IPropertyRepository propertyRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _propertyRepository = propertyRepository;
            _userRepository = userRepository;
        }

        public async Task<PaginatedResponse<PropertyReadDTO>> QueryPropertyAsync(PropertyQuery query)
        {
            var properties = await _propertyRepository.QueryPropertyAsync(query);
            return PaginatedResponse<PropertyReadDTO>.FromEnumerableWithMapping(properties, query, _mapper);
        }
        
        public async Task<IEnumerable<PropertyReadDTO>> ListAllAsync()
        {
            var properties = await _propertyRepository.GetAllAsync();
            return _mapper.Map<List<PropertyReadDTO>>(properties.ToList());
        }
        
        public async Task<PaginatedResponse<PropertyReadDTO>> QueryPropertyIgnoreStatusAsync(PropertyQuery query)
        {
            var properties = await _propertyRepository.QueryPropertyIgnoreStatusAsync(query);
            return PaginatedResponse<PropertyReadDTO>.FromEnumerableWithMapping(properties, query, _mapper);
        }
        
        public async Task<PropertyReadDTO> GetPropertyByIdAsync(int propertyId)
        {
            var property = await _propertyRepository.FindPropertyByIdAsync(propertyId);
            if (property == null)
            {
                throw new EntityWithIDNotFoundException<Property>(propertyId);
            }
            
            return _mapper.Map<PropertyReadDTO>(property);
        }

        public async Task<PropertyReadDTO> CreatePropertyAsync(string userId, PropertyWriteDTO propertyWriteDTO)
        {
            //Check if user exist
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }

            var property = _mapper.Map<Property>(propertyWriteDTO);
            
            await _propertyRepository.AddAsync(property);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<PropertyReadDTO>(property);
        }
        
        public async Task<PropertyReadDTO> UpdatePropertyDetailsAsync(int propertyId, PropertyWriteDTO propertyWriteDTO)
        {
            var property = await _propertyRepository.FindPropertyByIdAsync(propertyId);

            if (property == null)
            {
                throw new EntityWithIDNotFoundException<Property>(propertyId);
            }

            _mapper.Map(propertyWriteDTO, property);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<PropertyReadDTO>(property);
        }
        
        public async Task ChangePropertyStatusAsync(int propertyId)
        {
            var property = await _propertyRepository.FindPropertyByIdAsync(propertyId);

            if (property == null)
            {
                throw new EntityWithIDNotFoundException<Property>(propertyId);
            }
            
            switch (property.IsActive)
            {
                case true:
                    property.IsActive = false;
                    break;
                case false:
                    property.IsActive = true;
                    break;
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task DeletePropertyAsync(int propertyId)
        {
            var property = await _propertyRepository.FindPropertyByIdAsync(propertyId);

            if (property == null)
            {
                throw new EntityWithIDNotFoundException<Property>(propertyId);
            }

            _propertyRepository.Delete(property);
            await _unitOfWork.CommitAsync();
        }
    }
}