using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.Notification
{
    public class PushNotification
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}