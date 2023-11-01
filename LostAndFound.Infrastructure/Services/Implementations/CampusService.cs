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
    public class CampusService : ICampusService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICampusRepository _CampusRepository;
        private readonly IUserRepository _userRepository;
        
        public CampusService(IMapper mapper, IUnitOfWork unitOfWork, ICampusRepository CampusRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _CampusRepository = CampusRepository;
            _userRepository = userRepository;
        }

        public async Task<PaginatedResponse<CampusReadDTO>> QueryCampusAsync(CampusQuery query)
        {
            var campuses = await _CampusRepository.QueryCampusAsync(query);
            return PaginatedResponse<CampusReadDTO>.FromEnumerableWithMapping(campuses, query, _mapper);
        }
        
        public async Task<IEnumerable<CampusReadDTO>> ListAllAsync()
        {
            var campuses = await _CampusRepository.GetAllAsync();
            return _mapper.Map<List<CampusReadDTO>>(campuses.ToList());
        }
        
        public async Task<PaginatedResponse<CampusReadDTO>> QueryCampusIgnoreStatusAsync(CampusQuery query)
        {
            var campuses = await _CampusRepository.QueryCampusIgnoreStatusAsync(query);
            return PaginatedResponse<CampusReadDTO>.FromEnumerableWithMapping(campuses, query, _mapper);
        }
        
        public async Task<CampusReadDTO> GetCampusByIdAsync(int CampusId)
        {
            var campus = await _CampusRepository.FindCampusByIdAsync(CampusId);
            if (campus == null)
            {
                throw new EntityWithIDNotFoundException<Core.Entities.Campus>(CampusId);
            }
            
            return _mapper.Map<CampusReadDTO>(campus);
        }

        public async Task<CampusReadDTO> CreateCampusAsync(string userId, CampusWriteDTO CampusWriteDTO)
        {
            //Check if user exist
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }

            var campus = _mapper.Map<Core.Entities.Campus>(CampusWriteDTO);
            
            await _CampusRepository.AddAsync(campus);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CampusReadDTO>(campus);
        }
        
        public async Task<CampusReadDTO> UpdateCampusDetailsAsync(int CampusId, CampusWriteDTO CampusWriteDTO)
        {
            var campus = await _CampusRepository.FindCampusByIdAsync(CampusId);

            if (campus == null)
            {
                throw new EntityWithIDNotFoundException<Core.Entities.Campus>(CampusId);
            }

            _mapper.Map(CampusWriteDTO, campus);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CampusReadDTO>(campus);
        }
        
        public async Task<CampusReadDTO> ChangeCampusStatusAsync(int CampusId)
        {
            var campus = await _CampusRepository.FindCampusByIdAsync(CampusId);

            if (campus == null)
            {
                throw new EntityWithIDNotFoundException<Core.Entities.Campus>(CampusId);
            }

            campus.IsActive = !campus.IsActive;

            await _unitOfWork.CommitAsync();
            return _mapper.Map<CampusReadDTO>(campus);
        }

        public async Task<CampusReadDTO> DeleteCampusAsync(int CampusId)
        {
            var campus = await _CampusRepository.FindCampusByIdAsync(CampusId);

            if (campus == null)
            {
                throw new EntityWithIDNotFoundException<Core.Entities.Campus>(CampusId);
            }

            _CampusRepository.Delete(campus);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CampusReadDTO>(campus);
        }
    }
}