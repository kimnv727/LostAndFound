using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IReportService
    {
        Task<ReportReadDTO> CreateReportAsync(string userId, ReportWriteDTO writeDTO);
        Task<ReportReadDTO> UpdateReportStatusAsync(int reportId, ReportStatus reportStatus);
        Task<PaginatedResponse<ReportReadDTO>> QueryReports(ReportQuery query);
        Task<ReportReadDTO> GetReportById(int id);
        Task<PaginatedResponse<ReportReadDTO>> GetReportByUserAndItemId(string userId, int itemId);
        Task<PaginatedResponse<ReportReadDTO>> GetReportByUserId(string userId);
        Task<PaginatedResponse<ReportReadDTO>> GetReportByItemId(int itemId);
    }
}
