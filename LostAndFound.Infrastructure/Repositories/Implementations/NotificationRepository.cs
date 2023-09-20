using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Comment;
using LostAndFound.Infrastructure.DTOs.Notification;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(LostAndFoundDbContext context) : base(context)
        {
        }
        
        public async Task<Notification> FindNotificationByIdAsync(int id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
        }
        
        public async Task<int> CountUnreadOfUserAsync(string userId)
        {
            var result = _context.Notifications.Where(n => n.UserId == userId && n.IsRead == false);
            return await Task.FromResult(result.Count());
        }
        
        public async Task<IEnumerable<Notification>> FindUnreadNotificationsOfUserAsync(string userId)
        {
            IQueryable<Notification> notifications = _context.Notifications.Where(n => n.UserId == userId && n.IsRead == false);

            return await Task.FromResult(notifications.ToList());
        }
        
        public async Task<IEnumerable<Notification>> FindAllNotificationsOfUserAsync(string userId)
        {
            IQueryable<Notification> notifications = _context.Notifications.Where(n => n.UserId == userId);

            return await Task.FromResult(notifications.ToList());
        }
    }
}