using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.API.Controllers
{
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        
        //TODO: Implement later
    }
}