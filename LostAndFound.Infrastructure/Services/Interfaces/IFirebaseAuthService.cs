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
        Task<UserDetailAuthenticateReadDTO> Authenticate(string token, string refreshToken);
    }
}