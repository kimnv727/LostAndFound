using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LostAndFound.API.Extensions.FirebaseCloudMessaging;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.DTOs.Notification;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Implementations;
using LostAndFound.Infrastructure.Services.Interfaces;
using Newtonsoft.Json;

namespace LostAndFound.API.Extensions
{
    public static class NotificationExtensions
    {
        public static async Task NotifyChatToUser(IUserDeviceService userDeviceService, INotificationService notificationService,
            string userId, string notificationTitle, string notificationContent)
        {
            //get user's all device tokens
            var userDevices = await userDeviceService.GetUserDevicesOfUserAsync(userId);
            if (userDevices.Count() > 0)
            {
                List<string> tokensList = new List<string>();
                foreach (var ud in userDevices)
                {
                    tokensList.Add(ud.Token);
                }

                //Create notification to send
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
                try
                {
                    //Send notification
                    var response = await FirebaseCloudMessageSender.SendMulticastAsync(multicastMessage);
                    await InvalidFcmTokenCollector.HandleMulticastBatchResponse(response, tokensList!, userDeviceService);

                    if (response.SuccessCount > 0)
                    {
                        //Store notification in db
                        var notificationWriteDTO = new NotificationWriteDTO
                        {
                            Title = notificationTitle,
                            Content = notificationContent,
                            NotificationType = NotificationType.Chat
                        };
                        await notificationService.CreateNotification(notificationWriteDTO, userId);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.InnerException.ToString());
                }
            }
        }

        public static async Task NotifyPostRepliedToUser(IUserDeviceService userDeviceService, INotificationService notificationService,
            string userId, string notificationTitle, string notificationContent)
        {
            //get user's all device tokens
            var userDevices = await userDeviceService.GetUserDevicesOfUserAsync(userId);
            if (userDevices.Count() > 0)
            {
                List<string> tokensList = new List<string>();
                foreach (var ud in userDevices)
                {
                    tokensList.Add(ud.Token);
                }

                //Create notification to send
                var multicastMessage = new FirebaseAdmin.Messaging.MulticastMessage
                {
                    Tokens = tokensList,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = notificationTitle,
                        Body = notificationContent,
                    },
                };
                try
                {
                    //Send notification
                    var response = await FirebaseCloudMessageSender.SendMulticastAsync(multicastMessage);
                    await InvalidFcmTokenCollector.HandleMulticastBatchResponse(response, tokensList!, userDeviceService);

                    if (response.SuccessCount > 0)
                    {
                        //Store notification in db
                        var notificationWriteDTO = new NotificationWriteDTO
                        {
                            Title = notificationTitle,
                            Content = notificationContent,
                            NotificationType = NotificationType.PostGotReplied
                        };
                        await notificationService.CreateNotification(notificationWriteDTO, userId);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.InnerException.ToString());
                }
            }
        }

        public static async Task NotifyCommentRepliedToUser(IUserDeviceService userDeviceService, INotificationService notificationService,
            string userId, string notificationTitle, string notificationContent)
        {
            //get user's all device tokens
            var userDevices = await userDeviceService.GetUserDevicesOfUserAsync(userId);
            if (userDevices.Count() > 0)
            {
                List<string> tokensList = new List<string>();
                foreach (var ud in userDevices)
                {
                    tokensList.Add(ud.Token);
                }

                //Create notification to send
                var multicastMessage = new FirebaseAdmin.Messaging.MulticastMessage
                {
                    Tokens = tokensList,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = notificationTitle,
                        Body = notificationContent,
                    },
                };
                try
                {
                    //Send notification
                    var response = await FirebaseCloudMessageSender.SendMulticastAsync(multicastMessage);
                    await InvalidFcmTokenCollector.HandleMulticastBatchResponse(response, tokensList!, userDeviceService);

                    if (response.SuccessCount > 0)
                    {
                        //Store notification in db
                        var notificationWriteDTO = new NotificationWriteDTO
                        {
                            Title = notificationTitle,
                            Content = notificationContent,
                            NotificationType = NotificationType.CommentGotReplied
                        };
                        await notificationService.CreateNotification(notificationWriteDTO, userId);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.InnerException.ToString());
                }
            }
        }

        public static async Task NotifyItemClaimedToUser(IUserDeviceService userDeviceService, INotificationService notificationService,
            string userId, string notificationTitle, string notificationContent)
        {
            //get user's all device tokens
            var userDevices = await userDeviceService.GetUserDevicesOfUserAsync(userId);
            if (userDevices.Count() > 0)
            {
                List<string> tokensList = new List<string>();
                foreach (var ud in userDevices)
                {
                    tokensList.Add(ud.Token);
                }

                //Create notification to send
                var multicastMessage = new FirebaseAdmin.Messaging.MulticastMessage
                {
                    Tokens = tokensList,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = notificationTitle,
                        Body = notificationContent,
                    },
                };
                try
                {
                    //Send notification
                    var response = await FirebaseCloudMessageSender.SendMulticastAsync(multicastMessage);
                    await InvalidFcmTokenCollector.HandleMulticastBatchResponse(response, tokensList!, userDeviceService);

                    if (response.SuccessCount > 0)
                    {
                        //Store notification in db
                        var notificationWriteDTO = new NotificationWriteDTO
                        {
                            Title = notificationTitle,
                            Content = notificationContent,
                            NotificationType = NotificationType.OwnItemClaim
                        };
                        await notificationService.CreateNotification(notificationWriteDTO, userId);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.InnerException.ToString());
                }
            }
        }

        public static async Task NotifyGiveawayResultToUser(IUserDeviceService userDeviceService, INotificationService notificationService,
            string userId, string notificationTitle, string notificationContent)
        {
            //get user's all device tokens
            var userDevices = await userDeviceService.GetUserDevicesOfUserAsync(userId);
            if (userDevices.Count() > 0)
            {
                List<string> tokensList = new List<string>();
                foreach (var ud in userDevices)
                {
                    tokensList.Add(ud.Token);
                }

                //Create notification to send
                var multicastMessage = new FirebaseAdmin.Messaging.MulticastMessage
                {
                    Tokens = tokensList,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = notificationTitle,
                        Body = notificationContent,
                    },
                };
                try
                {
                    //Send notification
                    var response = await FirebaseCloudMessageSender.SendMulticastAsync(multicastMessage);
                    await InvalidFcmTokenCollector.HandleMulticastBatchResponse(response, tokensList!, userDeviceService);

                    if (response.SuccessCount > 0)
                    {
                        //Store notification in db
                        var notificationWriteDTO = new NotificationWriteDTO
                        {
                            Title = notificationTitle,
                            Content = notificationContent,
                            NotificationType = NotificationType.GiveawayResult
                        };
                        await notificationService.CreateNotification(notificationWriteDTO, userId);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.InnerException.ToString());
                }
            }
        }

        public static async Task NotifyRecommendItemToUser(IUserDeviceService userDeviceService, INotificationService notificationService,
            string userId, string notificationTitle, string notificationContent)
        {
            //get user's all device tokens
            var userDevices = await userDeviceService.GetUserDevicesOfUserAsync(userId);
            if (userDevices.Count() > 0)
            {
                List<string> tokensList = new List<string>();
                foreach (var ud in userDevices)
                {
                    tokensList.Add(ud.Token);
                }

                //Create notification to send
                var multicastMessage = new FirebaseAdmin.Messaging.MulticastMessage
                {
                    Tokens = tokensList,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = notificationTitle,
                        Body = notificationContent,
                    },
                };
                try
                {
                    //Send notification
                    var response = await FirebaseCloudMessageSender.SendMulticastAsync(multicastMessage);
                    await InvalidFcmTokenCollector.HandleMulticastBatchResponse(response, tokensList!, userDeviceService);

                    if (response.SuccessCount > 0)
                    {
                        //Store notification in db
                        var notificationWriteDTO = new NotificationWriteDTO
                        {
                            Title = notificationTitle,
                            Content = notificationContent,
                            NotificationType = NotificationType.RecommendItem
                        };
                        await notificationService.CreateNotification(notificationWriteDTO, userId);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.InnerException.ToString());
                }
            }
        }

        public static async Task NotifyRecommendPostToUser(IUserDeviceService userDeviceService, INotificationService notificationService,
            string userId, string notificationTitle, string notificationContent)
        {
            //get user's all device tokens
            var userDevices = await userDeviceService.GetUserDevicesOfUserAsync(userId);
            if (userDevices.Count() > 0)
            {
                List<string> tokensList = new List<string>();
                foreach (var ud in userDevices)
                {
                    tokensList.Add(ud.Token);
                }

                //Create notification to send
                var multicastMessage = new FirebaseAdmin.Messaging.MulticastMessage
                {
                    Tokens = tokensList,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = notificationTitle,
                        Body = notificationContent,
                    },
                };
                try
                {
                    //Send notification
                    var response = await FirebaseCloudMessageSender.SendMulticastAsync(multicastMessage);
                    await InvalidFcmTokenCollector.HandleMulticastBatchResponse(response, tokensList!, userDeviceService);

                    if (response.SuccessCount > 0)
                    {
                        //Store notification in db
                        var notificationWriteDTO = new NotificationWriteDTO
                        {
                            Title = notificationTitle,
                            Content = notificationContent,
                            NotificationType = NotificationType.RecommendPost
                        };
                        await notificationService.CreateNotification(notificationWriteDTO, userId);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.InnerException.ToString());
                }
            }
        }
    }
}