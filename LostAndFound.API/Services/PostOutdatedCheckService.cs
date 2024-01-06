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
    public class PostOutdatedCheckService : IHostedService, IDisposable
    {
        public IServiceProvider Services { get; }
        private readonly ILogger<PostOutdatedCheckService>? _logger;
        private Timer _timer = null!;
        public PostOutdatedCheckService(ILogger<PostOutdatedCheckService> logger, IServiceProvider services)
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
            _timer = new Timer(CheckPostStatusAsync, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            _logger!.LogInformation("Timer for post status check started.");
            return Task.CompletedTask;
        }

        private async void CheckPostStatusAsync(object? state)
        {
            using (var scope = Services.CreateScope())
            {
                try
                {
                    _logger!.LogInformation("Checking post status.");
                    var postRepository = scope.ServiceProvider.GetRequiredService<IPostRepository>();
                    List<Post> posts = (await postRepository.GetAllActivePosts()).ToList();
                    foreach (var post in posts)
                    {
                        try
                        {
                            //4 months -> outdated
                            if (DateTime.Now.ToVNTime() >= post.CreatedDate.AddDays(120))
                            {
                                post.PostStatus = PostStatus.EXPIRED;
                            }
                        }
                        catch
                        {
                            _logger!.LogInformation("Skipping faulty data.");
                            continue;
                        }
                    }
                    await postRepository.UpdatePostRange(posts.ToArray());
                }
                catch (Exception e)
                {
                    _logger!.LogInformation("Post status check operation was not successful.");
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
