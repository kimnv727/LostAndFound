using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Notification;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public NotificationService(IMapper mapper, IUnitOfWork unitOfWork, INotificationRepository notificationRepository,
            IUserRepository userRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
        }
        
        public async Task<NotificationReadDTO> GetNotification(int notificationId)
        {
            var noti = await _notificationRepository.FindNotificationByIdAsync(notificationId);

            if (noti == null)
            {
                throw new EntityWithIDNotFoundException<Notification>(notificationId);
            }

            return _mapper.Map<NotificationReadDTO>(noti);
        }
        
        public async Task<NotificationReadDTO> CreateNotification(NotificationWriteDTO notificationWriteDTO, string userId)
        {
            //Get User
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //Map Notification
            var notification = _mapper.Map<Notification>(notificationWriteDTO);
            notification.UserId = userId;
            //Add Notification
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.CommitAsync();
            var notificationReadDto = _mapper.Map<NotificationReadDTO>(notification);
            return notificationReadDto;
        }
        
        public async Task<int> CountUnreadOfUser(string userId)
        {
            //Get User
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //count
            var result = await _notificationRepository.CountUnreadOfUserAsync(userId);

            return result;
        }
        
        public async Task<PaginatedResponse<NotificationReadDTO>> GetUnreadNotificationsOfUser(string userId)
        {
            //check User exist
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //Get Unread Notification
            var result = await _notificationRepository.FindUnreadNotificationsOfUserAsync(userId);

            return _mapper.Map<PaginatedResponse<NotificationReadDTO>>(result.ToList());
        }
        
        public async Task<PaginatedResponse<NotificationReadDTO>> GetAllNotificationsOfUser(string userId)
        {
            //check User exist
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //Get Unread Notification
            var result = await _notificationRepository.FindAllNotificationsOfUserAsync(userId);

            return _mapper.Map<PaginatedResponse<NotificationReadDTO>>(result.ToList());
        }
        
        public async Task MarkAsRead(int id)
        {
            //Get Notification
            var notification = await _notificationRepository.FindNotificationByIdAsync(id);
            if (notification == null)
            {
                throw new EntityWithIDNotFoundException<Notification>(id);
            }

            notification.IsRead = true;
            await _unitOfWork.CommitAsync();
        }
        
        public async Task MarkAllAsRead(string userId)
        {
            //check User exist
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //Get Unread Notification
            var unread = await _notificationRepository.FindAllNotificationsOfUserAsync(userId);
            //Mark all as Read
            foreach (var u in unread)
            {
                u.IsRead = true;
            }
            await _unitOfWork.CommitAsync();
        }
    }
}