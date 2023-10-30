using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.ViolationReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IReportService
    {
        Task<ReportReadDTO> CreateReportAsync(CreateReportDTO report, string userId);

        Task<PaginatedResponse<ReportReadDTO>> QueryViolationReport(ReportQuery query);

        Task<ReportReadDTO> GetReportById(int id);
    }
}
