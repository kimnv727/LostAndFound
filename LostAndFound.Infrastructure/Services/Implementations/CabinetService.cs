using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Cabinet;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.Cabinet;
using LostAndFound.Infrastructure.DTOs.Common;
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
    public class CabinetService : ICabinetService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageRepository _storageRepository;
        private readonly ICabinetRepository _cabinetRepository;
        private readonly IItemRepository _itemRepository;

        public CabinetService(IMapper mapper, IUnitOfWork unitOfWork, IStorageRepository storageRepository,
            ICabinetRepository cabinetRepository, IItemRepository itemRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _storageRepository = storageRepository;
            _cabinetRepository = cabinetRepository;
            _itemRepository = itemRepository;
        }
        public async Task<CabinetReadDTO> CreateCabinetAsync(CabinetWriteDTO cabinetWriteDTO)
        {
            //Get Storage
            var storage = await _storageRepository.FindStorageByIdIncludeCabinetsAsync(cabinetWriteDTO.StorageId);
            if (storage == null)
            {
                throw new EntityWithIDNotFoundException<Storage>(cabinetWriteDTO.StorageId);
            }
            //check Name
            foreach (var c in storage.Cabinets)
            {
                if (c.Name == cabinetWriteDTO.Name)
                {
                    throw new FieldNotUniqueException("Name");
                }
            }
            //Map Cabinet 
            var cabinet = _mapper.Map<Cabinet>(cabinetWriteDTO);

            //Create Cabinet
            await _cabinetRepository.AddAsync(cabinet);
            await _unitOfWork.CommitAsync();
            var cabinetReadDTO = _mapper.Map<CabinetReadDTO>(cabinet);
            return cabinetReadDTO;
        }

        public async Task<CabinetReadDTO> UpdateCabinetStatusAsync(int cabinetId)
        {
            var cabinet = await _cabinetRepository.FindCabinetByIdIgnoreStatusAsync(cabinetId);
            if (cabinet == null)
            {
                throw new EntityWithIDNotFoundException<Cabinet>(cabinetId);
            }

            if(cabinet.IsActive == true)
            {
                if (cabinet.Items.ToList().Count > 0)
                {
                    foreach (var item in cabinet.Items)
                    {
                        if (item.ItemStatus == Core.Enums.ItemStatus.ACTIVE ||
                            item.ItemStatus == Core.Enums.ItemStatus.PENDING ||
                            item.ItemStatus == Core.Enums.ItemStatus.REJECTED)
                        {
                            throw new CabinetStillHaveItemException();
                        }
                    }
                }
            }

            cabinet.IsActive = !cabinet.IsActive;
            await _unitOfWork.CommitAsync();
            var cabinetReadDTO = _mapper.Map<CabinetReadDTO>(cabinet);
            return cabinetReadDTO;
        }

        public async Task<IEnumerable<CabinetReadDTO>> GetAllCabinetsByStorageIdAsync(int storageId)
        {
            //Get Storage
            var storage = await _storageRepository.FindStorageByIdAsync(storageId);
            if (storage == null)
            {
                throw new EntityWithIDNotFoundException<Storage>(storageId);
            }
            //Get Cabinets
            var cabinets = await _cabinetRepository.FindAllCabinetsByStorageIdAsync(storageId);

            return _mapper.Map<List<CabinetReadDTO>>(cabinets);
        }

        public async Task<IEnumerable<CabinetReadDTO>> GetAllCabinetsByStorageIdIgnoreStatusAsync(int storageId)
        {
            //Get Storage
            var storage = await _storageRepository.FindStorageByIdIgnoreStatusAsync(storageId);
            if (storage == null)
            {
                throw new EntityWithIDNotFoundException<Storage>(storageId);
            }
            //Get Cabinets
            var cabinets = await _cabinetRepository.FindAllCabinetsByStorageIdIgnoreStatusAsync(storageId);

            return _mapper.Map<List<CabinetReadDTO>>(cabinets);
        }

        public async Task<IEnumerable<CabinetReadDTO>> ListAllCabinetsAsync()
        {
            //Get Cabinets
            var cabinets = await _cabinetRepository.ListAllCabinetsAsync();

            return _mapper.Map<List<CabinetReadDTO>>(cabinets);
        }

        public async Task<CabinetReadDTO> GetCabinetByIdAsync(int id)
        {
            var cabinet = await _cabinetRepository.FindCabinetByIdAsync(id);

            if (cabinet == null)
            {
                throw new EntityWithIDNotFoundException<Cabinet>(id);
            }

            return _mapper.Map<CabinetReadDTO>(cabinet);
        }

        public async Task<CabinetReadDTO> GetCabinetByIdIgnoreStatusAsync(int id)
        {
            var cabinet = await _cabinetRepository.FindCabinetByIdIgnoreStatusAsync(id);

            if (cabinet == null)
            {
                throw new EntityWithIDNotFoundException<Cabinet>(id);
            }

            return _mapper.Map<CabinetReadDTO>(cabinet);
        }

        public async Task<PaginatedResponse<CabinetReadDTO>> QueryCabinetAsync(CabinetQuery query)
        {
            var cabinets = await _cabinetRepository.QueryCabinetAsync(query);

            return PaginatedResponse<CabinetReadDTO>.FromEnumerableWithMapping(cabinets, query, _mapper);
        }

        public async Task<CabinetReadDTO> UpdateCabinetDetailsAsync(int cabinetId, CabinetUpdateDTO cabinetUpdateDTO)
        {
            var cabinet = await _cabinetRepository.FindCabinetByIdAsync(cabinetId);
            if (cabinet == null)
            {
                throw new EntityWithIDNotFoundException<Cabinet>(cabinetId);
            }

            //check Storage
            var storage = await _storageRepository.FindStorageByIdIncludeCabinetsAsync(cabinetUpdateDTO.StorageId);
            if (storage == null)
            {
                throw new EntityWithIDNotFoundException<Storage>(cabinetUpdateDTO.StorageId);
            }

            //check Name
            foreach(var c in storage.Cabinets)
            {
                if(c.Name == cabinetUpdateDTO.Name && c.Id != cabinetId)
                {
                    throw new FieldNotUniqueException("Name");
                }
            }

            _mapper.Map(cabinetUpdateDTO, cabinet);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CabinetReadDTO>(cabinet);
        }
    }
}
