using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.User;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        Task<PaginatedResponse<UserDetailsReadDTO>> GetAllUsersAsync(UserQuery query);
        Task<PaginatedResponse<UserDetailsReadDTO>> GetAllUsersIgnoreStatusAsync(UserQueryIgnoreStatus query);
        Task<PaginatedResponse<UserDetailsReadDTO>> GetAllUsersIgnoreStatusWithoutWaitingVerifiedAsync(UserQueryIgnoreStatusWithoutWaitingVerified query);
        Task<UserDetailsReadDTO> GetUserAsync(string userID);
        Task<UserDetailsReadDTO> GetUserByEmailAsync(string email);
        Task RequestResetPassword(UserRequestResetPasswordDTO userRequestResetPasswordDTO);
        Task<UserDetailsReadDTO> CreateUserAsync(UserWriteDTO userWriteDTO);
        Task<UserDetailsReadDTO> ChangeUserStatusAsync(string id);
        Task<UserDetailsReadDTO> UpdateUserDetailsAsync(string userId, UserUpdateDTO updateDTO);
        //Task<UserDetailsReadDTO> UpdateUserDetailsAsync(string Id, UserWriteDTO writeDTO);
        Task<UserDetailsReadDTO> UpdateUserPasswordAsync(string userId, UserUpdatePasswordDTO updatePasswordDTO);
        Task<UserDetailsReadDTO> UpdateUserPasswordAndSendEmailAsync(string userId, UserUpdatePasswordDTO updatePasswordDTO);
        Task<bool> CheckUserExisted(string userId);
        Task<UserDetailsReadDTO> ChangeUserVerifyStatusAsync(UserVerifyStatusUpdateDTO updateDto);
        Task<IEnumerable<UserDetailsReadDTO>> ListAllStorageManagersAsync();
        Task<IEnumerable<UserDetailsReadDTO>> ListAllStorageManagersByCampusIdAsync(int campusId);
    }
}