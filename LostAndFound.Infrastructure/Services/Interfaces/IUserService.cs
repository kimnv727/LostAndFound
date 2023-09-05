using System;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.User;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        Task<PaginatedResponse<UserDetailsReadDTO>> GetAllUsersAsync(UserQuery query);
        Task<UserDetailsReadDTO> GetUserAsync(string userID);
        Task<UserDetailsReadDTO> GetUserByEmailAsync(string email);
        Task RequestResetPassword(UserRequestResetPasswordDTO userRequestResetPasswordDTO);
        Task<UserDetailsReadDTO> CreateUserAsync(UserWriteDTO userWriteDTO);
        Task ChangeUserStatusAsync(string id);
        Task<UserDetailsReadDTO> UpdateUserDetailsAsync(string userId, UserUpdateDTO updateDTO);
        Task<UserDetailsReadDTO> UpdateUserDetailsAsync(string Id, UserWriteDTO writeDTO);
        Task<UserDetailsReadDTO> UpdateUserPasswordAsync(string userId, UserUpdatePasswordDTO updatePasswordDTO);
        Task<UserDetailsReadDTO> UpdateUserPasswordAndSendEmailAsync(string userId, UserUpdatePasswordDTO updatePasswordDTO);
    }
}