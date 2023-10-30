using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.ViolationReport;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IReportRepository : IAddAsync<Report>, IDelete<Report>
    {
        Task<Int32> GetLastestCreatedReportIdAsync();

        Task<Report> GetLastestCreatedReportAsync();

        Task<IEnumerable<Report>> QueryAsync
            (ReportQuery query, bool trackChanges = false);

        Task<Report> GetReportByIdAsync(int id);
    }
}
