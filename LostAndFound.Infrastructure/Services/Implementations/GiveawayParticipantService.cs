using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Core.Exceptions.Giveaway;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.DTOs.GiveawayParticipant;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class GiveawayParticipantService : IGiveawayParticipantService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IGiveawayRepository _giveawayRepository;
        private readonly IGiveawayParticipantRepository _giveawayParticipantRepository;

        public GiveawayParticipantService(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository,
            IGiveawayRepository giveawayRepository, IGiveawayParticipantRepository giveawayParticipantRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _giveawayRepository = giveawayRepository;
            _giveawayParticipantRepository = giveawayParticipantRepository;
        }
        
        public async Task<int> CountGiveawayParticipantsAsync(int giveawayId)
        {
            //check Giveaway
            var giveaway = await _giveawayRepository.FindGiveawayByIdAsync(giveawayId);
            if (giveaway == null)
            {
                throw new EntityWithIDNotFoundException<Giveaway>(giveawayId);
            }
            //count
            var result = await _giveawayParticipantRepository.CountGiveawayParticipantsAsync(giveawayId);

            return result;
        }
        
        public async Task<GiveawayParticipantReadDTO> GetGiveawayParticipant(int giveawayId, string userId)
        {
            //check User
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //check Giveaway
            var giveaway = await _giveawayRepository.FindGiveawayByIdAsync(giveawayId);
            if (giveaway == null)
            {
                throw new EntityWithIDNotFoundException<Giveaway>(giveawayId);
            }
            
            var giveawayParticipant = await _giveawayParticipantRepository.FindGiveawayParticipantAsync(giveawayId, userId);

            if (giveawayParticipant == null)
            {
                throw new EntityNotFoundException<GiveawayParticipant>();
            }

            return _mapper.Map<GiveawayParticipantReadDTO>(giveawayParticipant);
        }
        
        public async Task<IEnumerable<UserReadDTO>> GetUsersParticipateInGiveaway(int giveawayId)
        {
            //check Giveaway
            var giveaway = await _giveawayRepository.FindGiveawayByIdAsync(giveawayId);
            if (giveaway == null)
            {
                throw new EntityWithIDNotFoundException<Giveaway>(giveawayId);
            }
            //Get Users participate in the Giveaway
            var result = await _giveawayParticipantRepository.FindUsersParticipateByGiveawayIdAsync(giveawayId);

            return _mapper.Map<List<UserReadDTO>>(result.ToList());
        }
        
        public async Task<GiveawayParticipantReadDTO> ParticipateInGiveaway(string userId, int giveawayId)
        {
            //check User 
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //check Giveaway
            var giveaway = await _giveawayRepository.FindGiveawayByIdAsync(giveawayId);
            if (giveaway == null)
            {
                throw new EntityWithIDNotFoundException<Giveaway>(giveawayId);
            }
            //check Giveaway available
            if (giveaway.GiveawayStatus != GiveawayStatus.ONGOING)
            {
                throw new Exception("This Giveaway is not available currently");
            }
            //Check Max Limit
            var participateCount = await _giveawayParticipantRepository.FindActiveGiveawayParticipantByUserIdAsync(userId);
            if (participateCount.Count() > 5)
            {
                throw new MaxParticipateLimit();
            }
            //check Giveaway time -> if pass then deny and closed
            if (giveaway.EndAt <= DateTime.Now.ToVNTime())
            {
                giveaway.GiveawayStatus = GiveawayStatus.CLOSED;
                await _unitOfWork.CommitAsync();
                throw new Exception("This Giveaway is not available currently");
            }
            //check GiveawayParticipant
            var giveawayParticipant = await _giveawayParticipantRepository.FindGiveawayParticipantAsync(giveawayId, userId);
            if (giveawayParticipant == null)
            {
                //giveawayParticipant not existed -> Create new one
                GiveawayParticipant gp = new GiveawayParticipant()
                {
                    GiveawayId = giveawayId,
                    UserId = userId,
                    IsActive = true,
                    IsWinner = false
                };
                await _giveawayParticipantRepository.AddAsync(gp);
                await _unitOfWork.CommitAsync();
                
                var returnResult = await _giveawayParticipantRepository.FindGiveawayParticipantAsync(giveawayId, userId);
                return _mapper.Map<GiveawayParticipantReadDTO>(returnResult); 
            }
            else
            {
                //giveawayParticipate already existed -> only change the bool status
                giveawayParticipant.IsActive = !giveawayParticipant.IsActive;
                await _unitOfWork.CommitAsync();
                return _mapper.Map<GiveawayParticipantReadDTO>(giveawayParticipant); 
            }
        }
    }
}