using System;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.User;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        Task<PaginatedResponse<UserDetailsReadDTO>> GetAllUsersAsync(UserQuery query);
        Task<UserDetailsReadDTO> GetUserAsync(Guid userID);
        Task<UserDetailsReadDTO> GetUserByEmailAsync(string email);
        Task RequestResetPassword(UserRequestResetPasswordDTO userRequestResetPasswordDTO);
        Task<UserDetailsReadDTO> CreateUserAsync(UserWriteDTO userWriteDTO);
        //Task<UserDetailsReadDTO> RegisterUserAsync(UserRegisterWriteDTO userRegisterWriteDTO);
        Task ChangeUserStatusAsync(Guid id);
        Task<UserDetailsReadDTO> UpdateUserDetailsAsync(Guid userID, UserUpdateDTO updateDTO);
        Task<UserDetailsReadDTO> UpdateUserDetailsAsync(Guid ID, UserWriteDTO writeDTO);
        Task<UserDetailsReadDTO> UpdateUserPasswordAsync(Guid userID, UserUpdatePasswordDTO updatePasswordDTO);
        Task<UserDetailsReadDTO> UpdateUserPasswordAndSendEmailAsync(Guid userID, UserUpdatePasswordDTO updatePasswordDTO);
    }
}