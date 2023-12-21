using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.UserDevice;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class UserDeviceService : IUserDeviceService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserDeviceRepository _userDeviceRepository;

        public UserDeviceService(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, IUserDeviceRepository userDeviceRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userDeviceRepository = userDeviceRepository;
        }
        
        public async Task<UserDeviceReadDTO> CreateUserDevice(UserDeviceWriteDTO userDeviceWriteDTO)
        {
            //Get User
            var user = await _userRepository.FindUserByID(userDeviceWriteDTO.UserId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userDeviceWriteDTO.UserId);
            }
            //Add User Device
            var userDevice = _mapper.Map<UserDevice>(userDeviceWriteDTO);
            await _userDeviceRepository.AddAsync(userDevice);
            await _unitOfWork.CommitAsync();
            var userDeviceReadDTO = _mapper.Map<UserDeviceReadDTO>(userDevice);
            return userDeviceReadDTO;
        }

        public async Task<UserDeviceReadDTO> UpdateUserDevice(string userId, string fcmToken)
        {
            //Get User
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            /*//Update User Device
            var userDevice = await _userDeviceRepository.FindUserDeviceByTokenAsync(fcmToken);
            if (userDevice != null)
            {
                userDevice.UserId = userId;
            }
            await _unitOfWork.CommitAsync();*/

            //Del first
            var result = await _userDeviceRepository.FindUserDeviceByTokenAsync(fcmToken);
            if (result != null)
            {
                _userDeviceRepository.Delete(result);
                await _unitOfWork.CommitAsync();
            }
            //Then Add
            var userDevice = new UserDevice
            {
                UserId = userId,
                Token = fcmToken
            };
            await _userDeviceRepository.AddAsync(userDevice);
            await _unitOfWork.CommitAsync();
            var userDeviceReadDTO = _mapper.Map<UserDeviceReadDTO>(userDevice);
            return userDeviceReadDTO;
        }

        public async Task<UserDeviceReadDTO> GetUserDeviceByTokenAsync(string fcmToken)
        {
            var userDevice = await _userDeviceRepository.FindUserDeviceByTokenAsync(fcmToken);
            //Do not throw error even when null
            if (userDevice == null)
            {
                return null;
                //throw new EntityNotFoundException<UserDevice>();
            }

            return _mapper.Map<UserDeviceReadDTO>(userDevice);
        }
        
        public async Task<IEnumerable<UserDeviceReadDTO>> GetUserDevicesOfUserAsync(string userId)
        {
            //Get User
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //Get UserDevices
            var userDevices = await _userDeviceRepository.FindUserDevicesOfUserAsync(userId);
            
            return _mapper.Map<PaginatedResponse<UserDeviceReadDTO>>(userDevices);
        }
        
        public async Task DeleteUserDeviceAsync(string fcmToken)
        {
            var userDevice = await _userDeviceRepository.FindUserDeviceByTokenAsync(fcmToken);

            if (userDevice == null)
            {
                throw new EntityNotFoundException<UserDevice>();
            }

            _userDeviceRepository.Delete(userDevice);
            await _unitOfWork.CommitAsync();
        }
    }
}