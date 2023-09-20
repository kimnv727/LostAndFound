using System;
using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.Notification
{
    public class NotificationReadDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}