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
using LostAndFound.Infrastructure.DTOs.User;
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
        private readonly IUserRepository _userRepository;

        public GiveawayService(IMapper mapper, IUnitOfWork unitOfWork, IGiveawayRepository giveawayRepository,
            IItemRepository itemRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _giveawayRepository = giveawayRepository;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
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
            var readDto = _mapper.Map<GiveawayReadDTO>(giveaway);
            foreach(var p in giveaway.GiveawayParticipants)
            {
                if(p.IsActive == true)
                {
                    readDto.ParticipantsCount++;
                }
                //only closed giveaway has winner
                if (p.IsWinner == true && p.IsChosenAsWinner == true && giveaway.GiveawayStatus == GiveawayStatus.CLOSED)
                {
                    var winner = await _userRepository.FindUserByID(p.UserId);
                    readDto.WinnerUser = _mapper.Map<UserReadDTO>(winner);
                }
            }
            return readDto;
        }

        public async Task<GiveawayDetailWithParticipantsReadDTO> GetGiveawayIncludeParticipantsByIdAsync(int giveawayId)
        {
            var giveaway = await _giveawayRepository.FindGiveawayIncludeParticipantsAsync(giveawayId);
            if (giveaway == null)
            {
                throw new EntityWithIDNotFoundException<Giveaway>(giveawayId);
            }

            var readDto = _mapper.Map<GiveawayDetailWithParticipantsReadDTO>(giveaway);
            foreach (var p in giveaway.GiveawayParticipants)
            {
                if (p.IsActive == true)
                {
                    readDto.ParticipantsCount++;
                }
                //only closed giveaway has winner
                if (p.IsWinner == true && p.IsChosenAsWinner == true && giveaway.GiveawayStatus == GiveawayStatus.CLOSED)
                {
                    var winner = await _userRepository.FindUserByID(p.UserId);
                    readDto.WinnerUser = _mapper.Map<UserReadDTO>(winner);
                }
            }
            return readDto;
        }

        public async Task<PaginatedResponse<GiveawayReadDTO>> QueryGiveawayAsync(GiveawayQuery query)
        {
            var giveaways = await _giveawayRepository.QueryGiveawayAsync(query);
            var result = PaginatedResponse<GiveawayReadDTO>.FromEnumerableWithMapping(giveaways, query, _mapper);
            foreach (var r in result)
            {
                foreach (var p in r.GiveawayParticipants)
                {
                    if (p.IsActive == true)
                    {
                        r.ParticipantsCount++;
                    }
                    //only closed giveaway has winner
                    if (p.IsWinner == true && p.IsChosenAsWinner == true && r.GiveawayStatus == GiveawayStatus.CLOSED)
                    {
                        r.WinnerUser = _mapper.Map<UserReadDTO>(p.User);
                    }
                }
            }
            //return PaginatedResponse<GiveawayReadDTO>.FromEnumerableWithMapping(giveaways, query, _mapper);
            return result;
        }

        public async Task<PaginatedResponse<GiveawayReadDTO>> QueryGiveawayWithStatusAsync(GiveawayQueryWithStatus query)
        {
            var giveaways = await _giveawayRepository.QueryGiveawayWithStatusAsync(query);

            var result = PaginatedResponse<GiveawayReadDTO>.FromEnumerableWithMapping(giveaways, query, _mapper);
            foreach (var r in result)
            {
                foreach (var p in r.GiveawayParticipants)
                {
                    if (p.IsActive == true)
                    {
                        r.ParticipantsCount++;
                    }
                    //only closed giveaway has winner
                    if (p.IsWinner == true && p.IsChosenAsWinner == true && r.GiveawayStatus == GiveawayStatus.CLOSED)
                    {
                        r.WinnerUser = _mapper.Map<UserReadDTO>(p.User);
                    }
                }
            }
            //return PaginatedResponse<GiveawayReadDTO>.FromEnumerableWithMapping(giveaways, query, _mapper);
            return result;
        }
        public async Task<PaginatedResponse<GiveawayReadDTO>> QueryGiveawayExcludeNotStartedAsync(GiveawayQueryExcludeNotStarted query)
        {
            var giveaways = await _giveawayRepository.QueryGiveawayExcludeNotstartedAsync(query);
            var result = PaginatedResponse<GiveawayReadDTO>.FromEnumerableWithMapping(giveaways, query, _mapper);
            foreach (var r in result)
            {
                foreach (var p in r.GiveawayParticipants)
                {
                    if (p.IsActive == true)
                    {
                        r.ParticipantsCount++;
                    }
                    //only closed giveaway has winner
                    if (p.IsWinner == true && p.IsChosenAsWinner == true && r.GiveawayStatus == GiveawayStatus.CLOSED)
                    {
                        r.WinnerUser = _mapper.Map<UserReadDTO>(p.User);
                    }
                }
            }
            //return PaginatedResponse<GiveawayReadDTO>.FromEnumerableWithMapping(giveaways, query, _mapper);
            return result;
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

            if(giveaway.StartAt > DateTime.Now)
            {
                giveaway.GiveawayStatus = GiveawayStatus.ONGOING;
            }
            else
            {
                giveaway.GiveawayStatus = GiveawayStatus.NOT_STARTED;
            }
            
            //Add Giveaway
            await _giveawayRepository.AddAsync(giveaway);
            await _unitOfWork.CommitAsync();

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