using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Auth.Providers;
using LostAndFound.Infrastructure.DTOs.Authenticate;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Services.Interfaces;

namespace LostAndFound.API.Authentication
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        private readonly FirebaseAuthClient _firebaseAuth;
        public FirebaseAuthService(FirebaseAuthClient firebaseAuth)
        {
            _firebaseAuth = firebaseAuth;
        }
        
        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest)
        {
            var userCredentials = await _firebaseAuth.SignInWithEmailAndPasswordAsync(loginRequest.Email, loginRequest.Password);

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
                return null;
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