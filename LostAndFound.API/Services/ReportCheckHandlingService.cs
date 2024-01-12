using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Extensions;
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
    public class ReportCheckHandlingService : IHostedService, IDisposable
    {
        public IServiceProvider Services { get; }
        private readonly ILogger<ReportCheckHandlingService>? _logger;
        private Timer _timer = null!;
        public ReportCheckHandlingService(ILogger<ReportCheckHandlingService> logger, IServiceProvider services)
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
            _logger!.LogInformation("Timer for report check started.");
            return Task.CompletedTask;
        }

        private async void CheckItemStatusAsync(object? state)
        {
            using (var scope = Services.CreateScope())
            {
                try
                {
                    _logger!.LogInformation("Checking report.");
                    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                    var reportRepository = scope.ServiceProvider.GetRequiredService<IReportRepository>();
                    List<Report> reports = (await reportRepository.GetAllSolvingReportAsync()).ToList();
                    foreach (var report in reports)
                    {
                        try
                        {
                            if(report.UpdatedDate != null)
                            {
                                if (DateTime.Now.ToVNTime() >= report.UpdatedDate?.AddDays(14))
                                {
                                    report.Status = ReportStatus.FAILED;
                                    report.ReportComment = "Report cannot be solved because the reported User with name " 
                                        + report.Item.ItemClaims.First().User.FullName
                                        + " did not cooperate.";
                                    //Ban User & send email
                                    await userService.ChangeUserStatusAsync(report.Item.ItemClaims.First().User.Id);
                                }
                            }
                        }
                        catch
                        {
                            _logger!.LogInformation("Skipping faulty data.");
                            continue;
                        }
                    }
                    await reportRepository.UpdateReportRange(reports.ToArray());
                }
                catch (Exception e)
                {
                    _logger!.LogInformation("Report check operation was not successful.");
                    _logger!.LogError(e, string.Empty, Array.Empty<int>());
                }
            }
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            _logger!.LogInformation("Timer for report check stopped.");
            return Task.CompletedTask;
        }
    }
}
