using System;
using System.Threading.Tasks;
using AutoMapper;
using Firebase.Auth;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Core.Exceptions.User;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;
using LostAndFound.Core.Entities;

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
        private readonly ICampusRepository _campusRepository;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, IPasswordHasherService passwordHasherService, 
            IEmailSendingService emailSendingService, FirebaseAuthClient firebaseAuth, ICampusRepository campusRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
            _emailSendingService = emailSendingService;
            _firebaseAuth = firebaseAuth;
            _campusRepository = campusRepository;
        }

        public async Task<PaginatedResponse<UserDetailsReadDTO>> GetAllUsersAsync(UserQuery query)
        {
            var users = await _userRepository.QueryUserAsync(query);
            
            return PaginatedResponse<UserDetailsReadDTO>.FromEnumerableWithMapping(users, query, _mapper);
        }
        
        public async Task<PaginatedResponse<UserDetailsReadDTO>> GetAllUsersIgnoreStatusAsync(UserQueryIgnoreStatus query)
        {
            var users = await _userRepository.QueryUserIgnoreStatusAsync(query);
            
            return PaginatedResponse<UserDetailsReadDTO>.FromEnumerableWithMapping(users, query, _mapper);
        }

        public async Task<PaginatedResponse<UserDetailsReadDTO>> GetAllUsersIgnoreStatusWithoutWaitingVerifiedAsync(UserQueryIgnoreStatusWithoutWaitingVerified query)
        {
            var users = await _userRepository.QueryUserIgnoreStatusWithoutWaitingVerifiedAsync(query);

            return PaginatedResponse<UserDetailsReadDTO>.FromEnumerableWithMapping(users, query, _mapper);
        }

        public async Task<UserDetailsReadDTO> GetUserAsync(string userId)
        {
            var user = await _userRepository.FindUserByID(userId);

            if (user == null)
            {
                throw new EntityWithIDNotFoundException<Core.Entities.User>(userId);
            }

            return _mapper.Map<UserDetailsReadDTO>(user);
        }
        
        public async Task<UserDetailsReadDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.FindUserByEmail(email);

            if (user == null)
            {
                throw new EntityWithEmailNotFoundException<Core.Entities.User>(email);
            }

            return _mapper.Map<UserDetailsReadDTO>(user);
        }

        public async Task<UserDetailsReadDTO> CreateUserAsync(UserWriteDTO userWriteDTO)
        {
            if (await _userRepository.IsDuplicatedEmail(userWriteDTO.Email))
            {
                throw new EmailAlreadyUsedException();
            }

            //check Campus
            var campus = await _campusRepository.FindCampusByIdAsync(userWriteDTO.CampusId);
            if (campus == null)
            {
                throw new EntityWithIDNotFoundException<Campus>(userWriteDTO.CampusId);
            }

            //Create User on Firebase
            try
            {
                //Check if email existed on Firebase here
                var result = await _firebaseAuth.FetchSignInMethodsForEmailAsync(userWriteDTO.Email);
                if (result.UserExists)
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
                    var user = _mapper.Map<Core.Entities.User>(userWriteDTO);
                    //Get FirebaseUID before create new User (this only for Admin created account)
                    user.Id = userCredentials.User.Uid;
                    user.VerifyStatus = UserVerifyStatus.VERIFIED;
                    await _userRepository.AddAsync(user);
                    await _unitOfWork.CommitAsync();
                    var userReadDTO = _mapper.Map<UserDetailsReadDTO>(user);
                    return userReadDTO;
                }
                else
                {
                    //Fail to Create User on Firebase
                    throw new FailToCreateUserException();
                }
            }
            catch (Exception e)
            {
                //Email already existed on Firebase
                throw new EmailAlreadyUsedException();
            }
        }

        //Update using updateDTO
        public async Task<UserDetailsReadDTO> UpdateUserDetailsAsync(string id, UserUpdateDTO updateDTO)
        {
            var user = await _userRepository.FindUserByID(id);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<Core.Entities.User>(id);
            }

            if (await _userRepository.IsDuplicatedPhoneNumber(updateDTO.Phone))
            {
                if (user.Phone != updateDTO.Phone)
                {
                    throw new PhoneNumberAlreadyUsedException();
                }
            }
            
            _mapper.Map(updateDTO, user);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<UserDetailsReadDTO>(user);
        }

        public async Task<UserDetailsReadDTO> UpdateUserPasswordAsync(string userId, UserUpdatePasswordDTO updatePasswordDTO)
        {
            var user = await _userRepository.FindUserByID(userId);
            
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<Core.Entities.User>(userId);
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

        public async Task<UserDetailsReadDTO> UpdateUserPasswordAndSendEmailAsync(string userId, UserUpdatePasswordDTO updatePasswordDTO)
        {
            var user = await _userRepository.FindUserByID(userId);

            if (user == null)
            {
                throw new EntityWithIDNotFoundException<Core.Entities.User>(userId);
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
                throw new EntityWithEmailNotFoundException<Core.Entities.User>(userRequestResetPasswordDTO.Email);
            }
            string pass = Guid.NewGuid().ToString("d").Substring(1, 16);
            user.Password = _passwordHasherService.HashPassword(pass);
            await _unitOfWork.CommitAsync();
            _emailSendingService.SendMailToRequestPasswordReset(user.Email, pass);
        }

        public async Task<UserDetailsReadDTO> ChangeUserStatusAsync(string id)
        {
            var user = await _userRepository.FindUserByID(id);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<Core.Entities.User>(id);
            }

            if(user.Role.Name == "Admin")
            {
                throw new AdminModificationException();
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

            return _mapper.Map<UserDetailsReadDTO>(user);
        }

        public async Task<bool> CheckUserExisted(string userId)
        {
            var user = await _userRepository.FindUserByID(userId);

            return user != null ? true : false;
        }
        
        public async Task<UserDetailsReadDTO> ChangeUserVerifyStatusAsync(UserVerifyStatusUpdateDTO updateDto)
        {
            var user = await _userRepository.FindUserByID(updateDto.UserId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<Core.Entities.User>(updateDto.UserId);
            }

            if (user.Role.Name == "Admin")
            {
                throw new AdminModificationException();
            }

            user.VerifyStatus = updateDto.VerifyStatus;
            await _unitOfWork.CommitAsync();
            
            return _mapper.Map<UserDetailsReadDTO>(user);
        }
    }
}