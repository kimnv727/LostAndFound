using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.UserDevice;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IUserDeviceService
    {
        //TODO: This will be used in UserController and NotificationExtensions
        Task<UserDeviceReadDTO> CreateUserDevice(UserDeviceWriteDTO userDeviceWriteDTO);
        Task<UserDeviceReadDTO> GetUserDeviceByTokenAsync(string fcmToken);
        Task<IEnumerable<UserDeviceReadDTO>> GetUserDevicesOfUserAsync(string userId);
        Task DeleteUserDeviceAsync(string fcmToken);
    }
}