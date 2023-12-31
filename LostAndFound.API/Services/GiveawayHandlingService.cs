using LostAndFound.API.Extensions;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Notification;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LostAndFound.API.Services
{
    public class GiveawayHandlingService : IHostedService, IDisposable
    {
        public IServiceProvider Services { get; }
        private readonly ILogger<GiveawayHandlingService>? _logger;
        private Timer _timer = null!;

        public GiveawayHandlingService(ILogger<GiveawayHandlingService> logger, IServiceProvider services)
        {
            _logger = logger;
            Services = services;
        }
        public void Dispose()
        {
            _timer?.Dispose();
            _logger!.LogInformation("Disposed timer for booking giveaway check.");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(GiveawayHandlingAsync, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            _logger!.LogInformation("Timer for giveaway status check started.");
            return Task.CompletedTask;
        }

        private async void GiveawayHandlingAsync(object? state)
        {
            using (var scope = Services.CreateScope())
            {
                try
                {
                    _logger!.LogInformation("Checking giveaway status.");
                    var giveawayRepository = scope.ServiceProvider.GetRequiredService<IGiveawayRepository>();
                    var giveawayParticipantRepository = scope.ServiceProvider.GetRequiredService<IGiveawayParticipantRepository>();
                    List<Giveaway> giveaways = (await giveawayRepository.GetAllOngoingGiveaways()).ToList();
                    List<Giveaway> notStartedGiveaways = (await giveawayRepository.GetAllNotStartedGiveaways()).ToList();
                    List<Giveaway> finishedGiveaways = new List<Giveaway>();
                    List<Giveaway> endingGiveaway = (await giveawayRepository.GetAllWaitingGiveaways()).ToList();
                    //Get ontime giveaway
                    foreach (var giveaway in notStartedGiveaways)
                    {
                        try
                        {
                            if (DateTime.Now >= giveaway.StartAt && giveaway.GiveawayStatus == GiveawayStatus.NOT_STARTED)
                            {
                                giveaway.GiveawayStatus = GiveawayStatus.ONGOING;
                            }
                        }
                        catch
                        {
                            _logger!.LogInformation("Skipping faulty data.");
                            continue;
                        }
                    }
                    await giveawayRepository.UpdateGiveawayRange(notStartedGiveaways.ToArray());

                    //Get finished (REWARD-DISTRIBUTION) giveaway
                    foreach (var giveaway in giveaways)
                    {
                        try
                        {
                            if (DateTime.Now >= giveaway.EndAt && giveaway.GiveawayStatus == GiveawayStatus.ONGOING)
                            {
                                
                                if(giveaway.GiveawayParticipants.Where(gp => gp.IsActive == true && gp.IsChosenAsWinner == false)
                                    .Count() > 0)
                                {
                                    giveaway.GiveawayStatus = GiveawayStatus.REWARD_DISTRIBUTION_IN_PROGRESS;
                                    finishedGiveaways.Add(giveaway);
                                }
                                else
                                {
                                    //No participant so just close
                                    giveaway.GiveawayStatus = GiveawayStatus.CLOSED;
                                }
                            }
                        }
                        catch
                        {
                            _logger!.LogInformation("Skipping faulty data.");
                            continue;
                        }
                    }
                    await giveawayRepository.UpdateGiveawayRange(giveaways.ToArray());

                    //Handling Participants to chose winner
                    foreach (var fg in finishedGiveaways)
                    {
                            var result = await giveawayRepository.FindGiveawayIncludeParticipantsAsync(fg.Id);
                            var winner = await giveawayParticipantRepository.RandomizeGiveawayWinnerAsync(fg.Id);
                            List<GiveawayParticipant> giveawayParticipants = result.GiveawayParticipants.ToList();
                            foreach (var gp in giveawayParticipants)
                            {
                                if(gp.IsChosenAsWinner == true && gp.IsWinner == true)
                                {
                                   gp.IsWinner = false;
                                }
                                if (gp.UserId == winner.UserId)
                                {
                                    //gp.IsActive = false;
                                    gp.IsWinner = true;
                                    gp.IsChosenAsWinner = true;
                                    //call noti api
                                    var noti = new PushNotification
                                    {
                                        UserId = gp.UserId,
                                        Title = "You win the Giveaway for Item named " + fg.Item.Name,
                                        Content = "You win the Giveaway for Item named " + fg.Item.Name + ". Congratulation!",
                                        NotificationType = NotificationType.GiveawayResult
                                    };
                                    await giveawayRepository.PushNotificationForGiveawayResult(noti);
                                    //also send mail
                                }
                                else
                                {
                                    //gp.IsActive = false;
                                    //gp.IsChosenAsWinner = false;
                                    //call noti api

                                    //Do nothing*************

                                    /*var noti = new PushNotification
                                    {
                                        UserId = gp.UserId,
                                        Title = "You do not win the Giveaway for Item named " + fg.Item.Name,
                                        Content = "You do not win the Giveaway for Item named " + fg.Item.Name + ". Good luck next time!",
                                        NotificationType = NotificationType.GiveawayResult
                                    };
                                    await giveawayRepository.PushNotificationForGiveawayResult(noti);*/
                                }
                            }
                            await giveawayParticipantRepository.UpdateGiveawayParticipantRange(giveawayParticipants.ToArray());
                    }

                    //Check Ending Giveaway to either REROLL OR CLOSE
                    foreach (var giveaway in endingGiveaway)
                    {
                        try
                        {
                            //check to see if its time to reroll yet?
                            var result = await giveawayRepository.FindGiveawayIncludeParticipantsAsync(giveaway.Id);
                            var chosenCount = result.GiveawayParticipants.Where(gp => gp.IsActive && gp.IsChosenAsWinner).Count();
                            if (DateTime.Now >= ((DateTime)result.EndAt).AddDays(3 * chosenCount))
                            {
                                if (giveaway.Item.ItemStatus != ItemStatus.GAVEAWAY)
                                {
                                    //reroll
                                    var winner = await giveawayParticipantRepository.RandomizeGiveawayWinnerAsync(giveaway.Id);
                                    if (winner != null)
                                    {
                                        List<GiveawayParticipant> giveawayParticipants = result.GiveawayParticipants.ToList();
                                        foreach (var gp in giveawayParticipants)
                                        {
                                            if (gp.IsChosenAsWinner == true && gp.IsWinner == true)
                                            {
                                                gp.IsWinner = false;
                                            }
                                            if (gp.UserId == winner.UserId)
                                            {
                                                //gp.IsActive = false;
                                                gp.IsWinner = true;
                                                gp.IsChosenAsWinner = true;
                                                //call noti api
                                                var noti = new PushNotification
                                                {
                                                    UserId = gp.UserId,
                                                    Title = "You win the Giveaway for Item named " + giveaway.Item.Name,
                                                    Content = "You win the Giveaway for Item named " + giveaway.Item.Name + ". Congratulation!",
                                                    NotificationType = NotificationType.GiveawayResult
                                                };
                                                await giveawayRepository.PushNotificationForGiveawayResult(noti);
                                                //also send mail
                                            }
                                        }
                                        await giveawayParticipantRepository.UpdateGiveawayParticipantRange(giveawayParticipants.ToArray());
                                    }
                                    else
                                    {
                                        //Closed because no more participants left
                                        giveaway.GiveawayStatus = GiveawayStatus.CLOSED;
                                    }

                                }
                                else if (giveaway.Item.ItemStatus == ItemStatus.GAVEAWAY)
                                {
                                    //Reward has been distributed -> Close Giveaway
                                    giveaway.GiveawayStatus = GiveawayStatus.CLOSED;
                                }
                            }
                        }
                        catch
                        {
                            _logger!.LogInformation("Skipping faulty data.");
                            continue;
                        }
                    }
                    await giveawayRepository.UpdateGiveawayRange(endingGiveaway.ToArray());

                }
                catch (Exception e)
                {
                    _logger!.LogInformation("Giveaway status check operation was not successful.");
                    _logger!.LogError(e, string.Empty, Array.Empty<int>());
                }
            }
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            _logger!.LogInformation("Timer for giveaway status check stopped.");
            return Task.CompletedTask;
        }
    }
}
