using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin.Auth;
using LostAndFound.Core.Exceptions.Authenticate;
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
        //TODO: Add Check for Login By Google (If not in DB then created, Otherwise make custom Claims (How?))
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
                        //TODO: Make custom Exception
                        //Either Wrong Email or Password
                        //PlaceHolderException
                        throw new Exception();
                    }
                }
                else
                {
                    //User not yet existed
                    //PlaceHolderException
                    throw new UnauthorizedException();
                }
            }
            catch (Exception e)
            {
                //Catch Wrong Email or Password 
                //PlaceHolderException
                throw new UnauthorizedException();
            }
        }
    
        public async Task Logout() => _firebaseAuth.SignOut(); 
        
        public async Task<UserDetailAuthenticateReadDTO> Authenticate(Guid userId)
        {
            /*var user = await _userRepository.FindUserByID(userId);
            if(user == null)
            {
                throw new UnauthorizedException();
            }
            return _mapper.Map<UserDetailAuthenticateReadDTO>(user);*/

            return null;
        }
    }
}