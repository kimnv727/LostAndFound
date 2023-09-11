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
    public interface IViolationReportService
    {
        Task<ViolationReportReadDTO> CreateReportAsync(CreateReportDTO report, string userId);

        Task<PaginatedResponse<ViolationReportReadDTO>> QueryViolationReport(ViolationReportQuery query);

        Task<ViolationReportReadDTO> GetReportById(int id);
    }
}
