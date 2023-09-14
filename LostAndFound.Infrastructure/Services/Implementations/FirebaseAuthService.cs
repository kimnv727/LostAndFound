using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin.Auth;
using LostAndFound.Core.Exceptions.Authenticate;
using LostAndFound.Core.Exceptions.User;
using LostAndFound.Infrastructure.DTOs.Authenticate;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.API.Authentication
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        private readonly FirebaseAuthClient _firebaseAuth;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public FirebaseAuthService(FirebaseAuthClient firebaseAuth, IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _firebaseAuth = firebaseAuth;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
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
        
        public async Task<UserDetailAuthenticateReadDTO> Authenticate(string token, string refreshToken)
        {
            //TODO: Is Password needed?
            //First
            //Check if user existed in DB -> If not then create (For GoogleLogin user)
            //Decode Token -> Get Info
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
                .VerifyIdTokenAsync(token);
            
            //Create User

            //Second
            //TODO: To Store Token in DB?

            return null;
        }
    }
}