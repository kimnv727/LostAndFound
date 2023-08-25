using System;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Authenticate;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IFirebaseAuthService
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest);
        Task Logout();
    }
}