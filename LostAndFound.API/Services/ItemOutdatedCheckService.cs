using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.Repositories.Interfaces;
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
    public class ItemOutdatedCheckService : IHostedService, IDisposable
    {
        public IServiceProvider Services { get; }
        private readonly ILogger<ItemOutdatedCheckService>? _logger;
        private Timer _timer = null!;
        public ItemOutdatedCheckService(ILogger<ItemOutdatedCheckService> logger, IServiceProvider services)
        {
            _logger = logger;
            Services = services;
        }
        public void Dispose()
        {
            _timer?.Dispose();
            _logger!.LogInformation("Disposed timer for booking status check.");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CheckItemStatusAsync, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            _logger!.LogInformation("Timer for item status check started.");
            return Task.CompletedTask;
        }

        private async void CheckItemStatusAsync(object? state)
        {
            using (var scope = Services.CreateScope())
            {
                try
                {
                    _logger!.LogInformation("Checking item status.");
                    var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();
                    List<Item> items = (await itemRepository.GetAllActiveItems()).ToList();
                    foreach (var item in items)
                    {
                        try
                        {
                            //4 months -> outdated
                            if (DateTime.Now.ToVNTime() >= item.CreatedDate.AddDays(120))
                            {
                                item.ItemStatus = ItemStatus.EXPIRED;
                            }
                        }
                        catch
                        {
                            _logger!.LogInformation("Skipping faulty data.");
                            continue;
                        }
                    }
                    await itemRepository.UpdateItemRange(items.ToArray());
                }
                catch (Exception e)
                {
                    _logger!.LogInformation("Item status check operation was not successful.");
                    _logger!.LogError(e, string.Empty, Array.Empty<int>());
                }
            }
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            _logger!.LogInformation("Timer for post status check stopped.");
            return Task.CompletedTask;
        }
    }
}
