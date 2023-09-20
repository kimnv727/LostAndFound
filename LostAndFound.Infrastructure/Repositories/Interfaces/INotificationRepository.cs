using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Notification;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface INotificationRepository :
        IGetAllAsync<Notification>,
        IAddAsync<Notification>,
        IUpdate<Notification>,
        IDelete<Notification>
    {
        Task<Notification> FindNotificationByIdAsync(int id);
        Task<int> CountUnreadOfUserAsync(string userId);
        Task<IEnumerable<Notification>> FindUnreadNotificationsOfUserAsync(string userId);
        Task<IEnumerable<Notification>> FindAllNotificationsOfUserAsync(string userId);
    }
}