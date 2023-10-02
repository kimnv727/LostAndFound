using System;
using System.Threading.Tasks;
using AutoMapper;
using Firebase.Auth;
using FirebaseAdmin.Auth;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.User;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.DTOs.Authenticate;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;
using User = LostAndFound.Core.Entities.User;

namespace LostAndFound.API.Authentication
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        private readonly FirebaseAuthClient _firebaseAuth;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserMediaRepository _userMediaRepository;
        public FirebaseAuthService(FirebaseAuthClient firebaseAuth, IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, IUserMediaRepository userMediaRepository)
        {
            _firebaseAuth = firebaseAuth;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userMediaRepository = userMediaRepository;
        }
        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest)
        {
            try
            {
                //Check in DB first, if not existed then Deny
                var user = await _userRepository.FindUserByEmail(loginRequest.Email);
                if (user != null)
                {
                    var userCredentials =
                        await _firebaseAuth.SignInWithEmailAndPasswordAsync(loginRequest.Email, loginRequest.Password);
                        
                    if (userCredentials != null)
                    {
                        var loginResponse = new LoginResponseDTO()
                        {
                            AccessToken = userCredentials.User.Credential.IdToken,
                            RefreshToken = userCredentials.User.Credential.RefreshToken
                        };
                        
                        return loginResponse;
                    }
                    else
                    {
                        //Either Wrong Email or Password
                        throw new WrongCredentialsException();
                    }
                }
                else
                {
                    //User not yet existed
                    throw new WrongCredentialsException();
                }
            }
            catch (Exception e)
            {
                //Catch Wrong Email or Password 
                throw new WrongCredentialsException();
            }
        }
    
        public async Task Logout() => _firebaseAuth.SignOut(); 
        
        public async Task<UserDetailAuthenticateReadDTO> Authenticate(string uid, string email, string name,
            string avatar, string phone)
        {
            //Check if user existed in DB -> If not then create (For GoogleLogin user)
            var user = await _userRepository.FindUserByID(uid);
            if (user != null)
            {
                return _mapper.Map<UserDetailAuthenticateReadDTO>(user);
            }
            else
            {
                //create User
                var newUser = new User()
                {
                    Id = uid,
                    Email = email,
                    Password = "",
                    IsActive = true,
                    Avatar = avatar,
                    FirstName = name,
                    LastName = " ",
                    //default to male
                    Gender = Core.Enums.Gender.Male,
                    Phone = phone,
                    //User role
                    RoleId = 3,
                    //User School Id
                    SchoolId = "",
                    //ProperId - default 1
                    //TODO: make dynamic later
                    PropertyId = 1,
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                await _userRepository.AddAsync(newUser);
                await _unitOfWork.CommitAsync();

                UserMedia userMedia = new UserMedia()
                {
                    UserId = uid,
                    Media = new Media()
                    {
                        Name = "GoogleAvatar",
                        Description = "Avatar of " + email,
                        URL = avatar,
                    }
                };
                await _userMediaRepository.AddAsync(userMedia);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<UserDetailAuthenticateReadDTO>(newUser);
            }
        }
    }
}