using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Report;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IReportRepository :
        IGetAllAsync<Report>,
        IAddAsync<Report>,
        IUpdate<Report>,
        IDelete<Report>
    {
        Task<IEnumerable<Report>> QueryAsync(ReportQuery query, bool trackChanges = false);
        Task<Report> GetReportByIdAsync(int reportId);
        Task<IEnumerable<Report>> CountTodayReportByUserIdAsync(string userId);
        Task<IEnumerable<Report>> GetReportByUserAndItemIdAsync(string userId, int itemId);
        Task<IEnumerable<Report>> GetReportsByUserIdAsync(string userId);
        Task<IEnumerable<Report>> GetReportsByItemIdAsync(int itemId);
        Task<IEnumerable<Report>> GetAllSolvingReportAsync();
        public Task UpdateReportRange(Report[] reports);
    }
}
