using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Authenticate;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Core.Exceptions.Storage;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Storage;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class StorageService : IStorageService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageRepository _storageRepository;
        private readonly ICabinetRepository _cabinetRepository;
        private readonly ICampusRepository _campusRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUserRepository _userRepository;

        public StorageService(IMapper mapper, IUnitOfWork unitOfWork, IStorageRepository storageRepository,
            ICabinetRepository cabinetRepository, ICampusRepository campusRepository, 
            ILocationRepository locationRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _storageRepository = storageRepository;
            _cabinetRepository = cabinetRepository;
            _campusRepository = campusRepository;
            _userRepository = userRepository;
            _locationRepository = locationRepository;
        }

        public async Task<StorageReadDTO> CreateStorageAsync(StorageWriteDTO storageWriteDTO)
        {
            //check Campus
            var campus = await _campusRepository.FindCampusByIdAsync(storageWriteDTO.CampusId);
            if (campus == null)
            {
                throw new EntityWithIDNotFoundException<Campus>(storageWriteDTO.CampusId);
            }
            //check manager
            var manager = await _userRepository.FindUserByID(storageWriteDTO.MainStorageManagerId);
            if(manager == null)
            {
                throw new EntityWithIDNotFoundException<User>(storageWriteDTO.MainStorageManagerId);
            }
            else if(manager.RoleId != 3)
            {
                throw new UnauthorizedException();
            }
            //check location unique
            var storages = await _storageRepository.FindAllStoragesByCampusIdAsync(storageWriteDTO.CampusId);
            foreach(var s in storages)
            {
                if (s.Location == storageWriteDTO.Location)
                {
                    throw new FieldNotUniqueException("Location");
                }
            }

            //Map Storage 
            var storage = _mapper.Map<Storage>(storageWriteDTO);

            //Create Storage
            await _storageRepository.AddAsync(storage);
            await _unitOfWork.CommitAsync();
            var cabinetReadDTO = _mapper.Map<StorageReadDTO>(storage);
            return cabinetReadDTO;
        }

        public async Task<IEnumerable<StorageReadDTO>> GetAllStoragesByCampusIdAsync(int campusId)
        {
            //Get Campus
            var campus = await _campusRepository.FindCampusByIdAsync(campusId);
            if (campus == null)
            {
                throw new EntityWithIDNotFoundException<Campus>(campusId);
            }
            //Get Storages
            var storages = await _storageRepository.FindAllStoragesByCampusIdAsync(campusId);

            return _mapper.Map<List<StorageReadDTO>>(storages);
        }

        public async Task<IEnumerable<StorageReadDTO>> GetAllStoragesByCampusIdIgnoreStatusAsync(int campusId)
        {
            //Get Campus
            var campus = await _campusRepository.FindCampusByIdAsync(campusId);
            if (campus == null)
            {
                throw new EntityWithIDNotFoundException<Campus>(campusId);
            }
            //Get Storages
            var storages = await _storageRepository.FindAllStoragesByCampusIdIgnoreStatusAsync(campusId);

            return _mapper.Map<List<StorageReadDTO>>(storages);
        }

        public async Task<IEnumerable<StorageReadIncludeCabinetsDTO>> GetAllStoragesByCampusIdIgnoreStatusIncludeCabinetsAsync(int campusId)
        {
            //Get Campus
            var campus = await _campusRepository.FindCampusByIdAsync(campusId);
            if (campus == null)
            {
                throw new EntityWithIDNotFoundException<Campus>(campusId);
            }
            //Get Storages
            var storages = await _storageRepository.FindAllStoragesByCampusIdIgnoreStatusIncludeCabinetsAsync(campusId);

            return _mapper.Map<List<StorageReadIncludeCabinetsDTO>>(storages);
        }

        public async Task<IEnumerable<StorageReadIncludeCabinetsDTO>> GetAllStoragesByCampusIdIncludeCabinetsAsync(int campusId)
        {
            //Get Campus
            var campus = await _campusRepository.FindCampusByIdAsync(campusId);
            if (campus == null)
            {
                throw new EntityWithIDNotFoundException<Campus>(campusId);
            }
            //Get Storages
            var storages = await _storageRepository.FindAllStoragesByCampusIdIncludeCabinetsAsync(campusId);

            return _mapper.Map<List<StorageReadIncludeCabinetsDTO>>(storages);
        }

        public async Task<StorageReadDTO> GetStorageByIdAsync(int id)
        {
            var storage = await _storageRepository.FindStorageByIdAsync(id);

            if (storage == null)
            {
                throw new EntityWithIDNotFoundException<Storage>(id);
            }

            return _mapper.Map<StorageReadDTO>(storage);
        }

        public async Task<StorageReadDTO> GetStorageByIdIgnoreStatusAsync(int id)
        {
            var storage = await _storageRepository.FindStorageByIdIgnoreStatusAsync(id);

            if (storage == null)
            {
                throw new EntityWithIDNotFoundException<Storage>(id);
            }

            return _mapper.Map<StorageReadDTO>(storage);
        }

        public async Task<StorageReadIncludeCabinetsDTO> GetStorageByIdIncludeCabinetsAsync(int id)
        {
            var storage = await _storageRepository.FindStorageByIdIncludeCabinetsAsync(id);

            if (storage == null)
            {
                throw new EntityWithIDNotFoundException<Storage>(id);
            }

            return _mapper.Map<StorageReadIncludeCabinetsDTO>(storage);
        }

        public async Task<StorageReadIncludeCabinetsDTO> GetStorageByIdIncludeCabinetsIgnoreStatusAsync(int id)
        {
            var storage = await _storageRepository.FindStorageByIdIncludeCabinetsIgnoreStatusAsync(id);

            if (storage == null)
            {
                throw new EntityWithIDNotFoundException<Storage>(id);
            }

            return _mapper.Map<StorageReadIncludeCabinetsDTO>(storage);
        }

        public async Task<PaginatedResponse<StorageReadIncludeCabinetsDTO>> QueryStorageAsync(StorageQuery query)
        {
            var storages = await _storageRepository.QueryStorageAsync(query);

            //get storage Manager
            var userQuery = new UserQuery();
            userQuery.Role = UserQuery.RoleSearch.Storage_Manager;
            var storageManagers = await _userRepository.QueryUserAsync(userQuery);

            foreach(var m in storageManagers)
            {
                foreach(var s in storages)
                {
                    if(s.MainStorageManagerId == m.Id)
                    {
                        s.MainStorageManagerId = m.Email;
                    }
                }
            }

            return PaginatedResponse<StorageReadIncludeCabinetsDTO>.FromEnumerableWithMapping(storages, query, _mapper);
        }

        public async Task<StorageReadDTO> UpdateStorageDetailsAsync(int storageId, StorageUpdateDTO storageUpdateDTO)
        {
            var storage = await _storageRepository.FindStorageByIdAsync(storageId);
            if (storage == null)
            {
                throw new EntityWithIDNotFoundException<Storage>(storageId);
            }
            //check manager
            var manager = await _userRepository.FindUserByID(storageUpdateDTO.MainStorageManagerId);
            if (manager == null)
            {
                throw new EntityWithIDNotFoundException<User>(storageUpdateDTO.MainStorageManagerId);
            }
            else if (manager.RoleId != 3)
            {
                throw new UnauthorizedAccessException();
            }
            //check location unique
            var storages = await _storageRepository.FindAllStoragesByCampusIdAsync(storageUpdateDTO.CampusId);
            foreach (var s in storages)
            {
                if (s.Location == storageUpdateDTO.Location && s.Id != storageId)
                {
                    throw new FieldNotUniqueException("Location");
                }
            }

            _mapper.Map(storageUpdateDTO, storage);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<StorageReadDTO>(storage);
        }

        public async Task<StorageReadDTO> UpdateStorageStatusAsync(int storageId)
        {
            var storage = await _storageRepository.FindStorageByIdIncludeCabinetsIgnoreStatusAsync(storageId);
            if (storage == null)
            {
                throw new EntityWithIDNotFoundException<Storage>(storage);
            }

            //check cabinets
            if(storage.IsActive == true)
            {
                if (storage.Cabinets.ToList().Count > 0)
                {
                    foreach(var cabin in storage.Cabinets)
                    {
                        if(cabin.IsActive == true)
                        {
                            throw new StorageStillHaveActiveCabinet();
                        }
                    }
                }
            }

            storage.IsActive = !storage.IsActive;
            await _unitOfWork.CommitAsync();
            var storageReadDTO = _mapper.Map<StorageReadDTO>(storage);
            return storageReadDTO;
        }

        public async Task<IEnumerable<StorageReadDTO>> ListAllStoragesAsync()
        {
            //Get Storages
            var storages = await _storageRepository.ListAllStoragesAsync();

            return _mapper.Map<List<StorageReadDTO>>(storages);
        }
    }
}
