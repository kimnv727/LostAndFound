using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.ViolationReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IViolationReportService
    {
        Task CreateReportAsync(CreateReportDTO report, string userId);
    }
}
