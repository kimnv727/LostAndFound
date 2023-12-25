using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Giveaway;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class GiveawayService : IGiveawayService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGiveawayRepository _giveawayRepository;
        private readonly IItemRepository _itemRepository;
        //TODO: add worker
        public GiveawayService(IMapper mapper, IUnitOfWork unitOfWork, IGiveawayRepository giveawayRepository,
            IItemRepository itemRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _giveawayRepository = giveawayRepository;
            _itemRepository = itemRepository;
        }

        public async Task UpdateGiveawayStatusAsync(int giveawayId, GiveawayStatus giveawayStatus)
        {
            var giveaway = await _giveawayRepository.FindGiveawayByIdAsync(giveawayId);
            if (giveaway == null)
            {
                throw new EntityWithIDNotFoundException<Giveaway>(giveawayId);
            }
            giveaway.GiveawayStatus = giveawayStatus;
            await _unitOfWork.CommitAsync();
        }

        public async Task<GiveawayReadDTO> GetGiveawayByIdAsync(int giveawayId)
        {
            var giveaway = await _giveawayRepository.FindGiveawayByIdAsync(giveawayId);
            if (giveaway == null)
            {
                throw new EntityWithIDNotFoundException<Giveaway>(giveawayId);
            }

            return _mapper.Map<GiveawayReadDTO>(giveaway);
        }

        public async Task<GiveawayDetailWithParticipantsReadDTO> GetGiveawayIncludeParticipantsByIdAsync(int giveawayId)
        {
            var giveaway = await _giveawayRepository.FindGiveawayIncludeParticipantssAsync(giveawayId);
            if (giveaway == null)
            {
                throw new EntityWithIDNotFoundException<Giveaway>(giveawayId);
            }

            return _mapper.Map<GiveawayDetailWithParticipantsReadDTO>(giveaway);
        }

        public async Task<PaginatedResponse<GiveawayReadDTO>> QueryGiveawayAsync(GiveawayQuery query)
        {
            var giveaways = await _giveawayRepository.QueryGiveawayAsync(query);
            
            return PaginatedResponse<GiveawayReadDTO>.FromEnumerableWithMapping(giveaways, query, _mapper);
        }

        public async Task<PaginatedResponse<GiveawayReadDTO>> QueryGiveawayWithStatusAsync(GiveawayQueryWithStatus query)
        {
            var giveaways = await _giveawayRepository.QueryGiveawayWithStatusAsync(query);
            
            return PaginatedResponse<GiveawayReadDTO>.FromEnumerableWithMapping(giveaways, query, _mapper);
        }

        public async Task<GiveawayReadDTO> CreateGiveawayAsync(GiveawayWriteDTO giveawayWriteDTO)
        {
            //Get Item
            var item = await _itemRepository.FindItemByIdAsync(giveawayWriteDTO.ItemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(giveawayWriteDTO.ItemId);
            }
            //Map Giveaway
            var giveaway = _mapper.Map<Giveaway>(giveawayWriteDTO);
            giveaway.GiveawayStatus = GiveawayStatus.NOT_STARTED;
            //Add Giveaway
            await _giveawayRepository.AddAsync(giveaway);
            await _unitOfWork.CommitAsync();
            //TODO: find way to return giveaway after create (maybe an extra repo function to return newest record)
            var giveawayReadDTO = _mapper.Map<GiveawayReadDTO>(giveaway);
            return giveawayReadDTO;
        }

        public async Task<GiveawayReadDTO> UpdateGiveawayDetailsAsync(int giveawayId, GiveawayUpdateDTO giveawayUpdateDTO)
        {
            var giveaway = await _giveawayRepository.FindGiveawayByIdAsync(giveawayId);
            if (giveaway == null)
            {
                throw new EntityWithIDNotFoundException<Giveaway>(giveawayId);
            }
            //Check if it is OnGoing
            if (giveaway.GiveawayStatus == GiveawayStatus.ONGOING)
            {
                throw new Exception("Cannot update ONGOING Giveaway");
            }
            _mapper.Map(giveawayUpdateDTO, giveaway);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<GiveawayReadDTO>(giveaway);
        }

        public async Task<IEnumerable<ItemReadDTO>> ListItemsSuitableForGiveawayAsync()
        {
            var items = await _giveawayRepository.GetAllItemsSuitableForGiveaway();

            return _mapper.Map<List<ItemReadDTO>>(items);
        }
    }
}