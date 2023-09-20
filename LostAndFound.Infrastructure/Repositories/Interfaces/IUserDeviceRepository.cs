using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IUserDeviceRepository :
        IGetAllAsync<UserDevice>,
        IAddAsync<UserDevice>,
        IUpdate<UserDevice>,
        IDelete<UserDevice>
    {
        Task<UserDevice> FindUserDeviceByTokenAsync(string fcmToken);
        Task<IEnumerable<UserDevice>> FindUserDevicesOfUserAsync(string userId);
    }
}