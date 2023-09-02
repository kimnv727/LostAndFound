using System;
using System.Threading.Tasks;
using AutoMapper;
using Firebase.Auth;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Core.Exceptions.User;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;
using User = LostAndFound.Core.Entities.User;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IEmailSendingService _emailSendingService;
        private readonly FirebaseAuthClient _firebaseAuth;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, IPasswordHasherService passwordHasherService, 
            IEmailSendingService emailSendingService, FirebaseAuthClient firebaseAuth)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
            _emailSendingService = emailSendingService;
            _firebaseAuth = firebaseAuth;
        }

        public async Task<PaginatedResponse<UserDetailsReadDTO>> GetAllUsersAsync(UserQuery query)
        {
            var users = await _userRepository.QueryAsync(query);
            
            return PaginatedResponse<UserDetailsReadDTO>.FromEnumerableWithMapping(users, query, _mapper);
        }
    
        public async Task<UserDetailsReadDTO> GetUserAsync(Guid userID)
        {
            var user = await _userRepository.FindUserByID(userID);

            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userID);
            }

            return _mapper.Map<UserDetailsReadDTO>(user);
        }
        
        public async Task<UserDetailsReadDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.FindUserByEmail(email);

            if (user == null)
            {
                throw new EntityWithEmailNotFoundException<User>(email);
            }

            return _mapper.Map<UserDetailsReadDTO>(user);
        }

        public async Task<UserDetailsReadDTO> CreateUserAsync(UserWriteDTO userWriteDTO)
        {
            if (await _userRepository.IsDuplicatedEmail(userWriteDTO.Email))
            {
                throw new EmailAlreadyUsedException();
            }
            //Create User on Firebase
            var userCredentials = await _firebaseAuth.CreateUserWithEmailAndPasswordAsync(userWriteDTO.Email, userWriteDTO.Password);
            if (userCredentials != null)
            {
                //Create on Firebase successfully
                //Create User in DB
                userWriteDTO.Password = _passwordHasherService.HashPassword(userWriteDTO.Password);
                var user = _mapper.Map<User>(userWriteDTO);
                //Get FirebaseUID before create new User (this only for Admin created account)
                user.FirebaseUID = userCredentials.User.Uid;
                await _userRepository.AddAsync(user);
                await _unitOfWork.CommitAsync();
                var userReadDTO = _mapper.Map<UserDetailsReadDTO>(user);
                return userReadDTO;
            }
            else
            {
                //Fail to Create User on Firebase
                //Placeholder Exception
                return null;
            }
        }

        //Update using updateDTO
        public async Task<UserDetailsReadDTO> UpdateUserDetailsAsync(Guid id, UserUpdateDTO updateDTO)
        {
            var user = await _userRepository.FindUserByID(id);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(id);
            }

            if (await _userRepository.IsDuplicatedPhoneNumber(updateDTO.PhoneNumber))
            {
                if (user.Phone != updateDTO.PhoneNumber)
                {
                    throw new PhoneNumberAlreadyUsedException();
                }
            }
            
            _mapper.Map(updateDTO, user);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<UserDetailsReadDTO>(user);
        }
        
        //Update using UpdateWriteDTO
        public async Task<UserDetailsReadDTO> UpdateUserDetailsAsync(Guid id, UserWriteDTO writeDTO)
        {
            var user = await _userRepository.FindUserByID(id);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(id);
            }
            
            _mapper.Map(writeDTO, user);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<UserDetailsReadDTO>(user);
        }

        public async Task<UserDetailsReadDTO> UpdateUserPasswordAsync(Guid userID, UserUpdatePasswordDTO updatePasswordDTO)
        {
            var user = await _userRepository.FindUserByID(userID);
            
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userID);
            }
            else if (!_passwordHasherService.VerifyCorrectPassword(
                    updatePasswordDTO.oldPassword, //Old password 
                    user.Password)) //Hashed old password
            {
                throw new WrongCredentialsException();
            }
            else if (!String.IsNullOrEmpty(updatePasswordDTO.NewPassword))
            {
                user.Password = _passwordHasherService.HashPassword(updatePasswordDTO.NewPassword);
            }
            
            _mapper.Map(updatePasswordDTO, user);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<UserDetailsReadDTO>(user);
        }

        public async Task<UserDetailsReadDTO> UpdateUserPasswordAndSendEmailAsync(Guid userID, UserUpdatePasswordDTO updatePasswordDTO)
        {
            var user = await _userRepository.FindUserByID(userID);

            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userID);
            }
            else if (!_passwordHasherService.VerifyCorrectPassword(
                    updatePasswordDTO.oldPassword, //Old password 
                    user.Password)) //Hashed old password
            {
                throw new WrongCredentialsException();
            }
            else if (!String.IsNullOrEmpty(updatePasswordDTO.NewPassword))
            {
                user.Password = _passwordHasherService.HashPassword(updatePasswordDTO.NewPassword);
            }

            _mapper.Map(updatePasswordDTO, user);
            await _unitOfWork.CommitAsync();
            _emailSendingService.SendMailInformSuccessPasswordChange(user.Email);
            return _mapper.Map<UserDetailsReadDTO>(user);
        }

        public async Task RequestResetPassword(UserRequestResetPasswordDTO userRequestResetPasswordDTO)
        {
            var user = await _userRepository.FindUserByEmail(userRequestResetPasswordDTO.Email);
            if (user == null)
            {
                throw new EntityWithEmailNotFoundException<User>(userRequestResetPasswordDTO.Email);
            }
            string pass = Guid.NewGuid().ToString("d").Substring(1, 16);
            user.Password = _passwordHasherService.HashPassword(pass);
            await _unitOfWork.CommitAsync();
            _emailSendingService.SendMailToRequestPasswordReset(user.Email, pass);
        }

        public async Task ChangeUserStatusAsync(Guid id)
        {
            var user = await _userRepository.FindUserByID(id);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(id);
            }

            if (user.IsActive == true)
            {
                user.IsActive = false;
            }
            else
            {
                user.IsActive = true;
            }
            await _unitOfWork.CommitAsync();
        }
    }
}