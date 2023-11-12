using LostAndFound.API.Extensions;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
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
        private readonly INotificationService _notificationService;
        private readonly IUserDeviceService _userDeviceService;

        public GiveawayHandlingService(ILogger<GiveawayHandlingService> logger, IServiceProvider services,
            INotificationService notificationService, IUserDeviceService userDeviceService)
        {
            _logger = logger;
            Services = services;
            _notificationService = notificationService;
            _userDeviceService = userDeviceService;
        }
        public void Dispose()
        {
            _timer?.Dispose();
            _logger!.LogInformation("Disposed timer for booking giveaway check.");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(GiveawayHandlingAsync, null, TimeSpan.Zero, TimeSpan.FromHours(1));
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
                    List<Giveaway> finishedGiveaways = new List<Giveaway>();
                    //Get finished giveaway
                    foreach (var giveaway in giveaways)
                    {
                        try
                        {
                            if (DateTime.Now >= giveaway.EndAt)
                            {
                                giveaway.GiveawayStatus = GiveawayStatus.CLOSED;
                            }
                            finishedGiveaways.Add(giveaway);

                        }
                        catch
                        {
                            _logger!.LogInformation("Skipping faulty data.");
                            continue;
                        }
                    }
                    await giveawayRepository.UpdateGiveawayRange(giveaways.ToArray());
                    //Handling Participants
                    foreach (var fg in finishedGiveaways)
                    {
                        var result = await giveawayRepository.FindGiveawayIncludeParticipantssAsync(fg.Id);
                        var winner = await giveawayParticipantRepository.RandomizeGiveawayWinnerAsync(fg.Id);
                        List<GiveawayParticipant> giveawayParticipants = result.GiveawayParticipants.ToList();
                        foreach(var gp in giveawayParticipants)
                        {
                            if(gp.UserId == winner.UserId)
                            {
                                gp.IsActive = false;
                                gp.IsWinner = true;
                                await NotificationExtensions
                                .NotifyGiveawayResultToUser(_userDeviceService, _notificationService, 
                                gp.UserId, "Giveaway " + fg.Id + " Result", "You are the winner of Giveaway " + fg.Id + "!");
                            }
                            else
                            {
                                gp.IsActive = false;
                                gp.IsWinner = false;
                                await NotificationExtensions
                                .NotifyGiveawayResultToUser(_userDeviceService, _notificationService,
                                gp.UserId, "Giveaway " + fg.Id + " Result", "You are not the winner of Giveaway " + fg.Id + "! Try again next time!");
                            }
                        }
                        await giveawayParticipantRepository.UpdateGiveawayParticipantRange(giveawayParticipants.ToArray());
                    }

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
