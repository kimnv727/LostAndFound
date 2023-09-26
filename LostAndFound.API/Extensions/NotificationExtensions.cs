using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LostAndFound.API.Extensions.FirebaseCloudMessaging;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Implementations;
using LostAndFound.Infrastructure.Services.Interfaces;
using Newtonsoft.Json;

namespace LostAndFound.API.Extensions
{
    public static class NotificationExtensions
    {
        //TODO: Implement later
        public static async Task NotifyChatToUser(IUserDeviceService userDeviceService,
            string userId, string notificationTitle, string notificationContent)
        {
            //var userDevices = await userDeviceService.GetUserDevicesOfUserAsync(userId);
            //if (userDevices.ToList().Count() > 0)
            //{
                List<string> tokensList = new List<string>();
                //foreach (var ud in userDevices)
                //{
                //    tokensList.Add(ud.Token);
                //}
                var token =
                    "e072XQnXGZ52YMIVlFIpKR:APA91bHZ8lBsFfgS0UwdKsCj5hheBVQ0CKE1XA6akzFHYNYxx4bK0ceadKL4R8hPHE4t5gwleGmk4RCJHxjWVavR_xNvaXSfq1l1hwk8PVmW8_Cisd3iRooSzEIPioE53NSfW2hl6B3J";
                tokensList.Add(token);
                var multicastMessage = new FirebaseAdmin.Messaging.MulticastMessage
                {
                    Tokens = tokensList,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = notificationTitle,
                        Body = notificationContent,
                    },
                    Data = new Dictionary<string, string>
                    {
                        { "notificationType", ((int)NotificationType.Chat).ToString() },
                        { "createdDate", DateTime.Now.ToVNTime().ToString() }
                    }
                };
                var response = await FirebaseCloudMessageSender.SendMulticastAsync(multicastMessage);
                await InvalidFcmTokenCollector.HandleMulticastBatchResponse(response, tokensList!, userDeviceService);
                //TODO: after this if success create notification to store in db
            //}
        }
    }
}