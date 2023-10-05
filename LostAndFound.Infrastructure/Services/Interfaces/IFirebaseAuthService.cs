using System;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Authenticate;
using LostAndFound.Infrastructure.DTOs.User;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IFirebaseAuthService
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest);
        Task Logout();
        Task<UserDetailAuthenticateReadDTO> Authenticate(string uid, string email, string name,
                                                    string avatar, string phone);

        Task<string> GetAccessTokenWithRefreshToken(string refreshToken);
    }
}