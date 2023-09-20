using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Notification;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationReadDTO> CreateNotification(NotificationWriteDTO notificationWriteDTO, string userId);
        Task<int> CountUnreadOfUser(string userId);
        Task<IEnumerable<NotificationReadDTO>> GetUnreadNotificationsOfUser(string userId);
        Task<IEnumerable<NotificationReadDTO>> GetAllNotificationsOfUser(string userId);
        Task MarkAsRead(int id);
        Task MarkAllAsRead(string userId);
    }
}