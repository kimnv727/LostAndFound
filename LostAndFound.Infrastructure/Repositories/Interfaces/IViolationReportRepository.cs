using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.ViolationReport;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IViolationReportRepository : IAddAsync<ViolationReport>, IDelete<ViolationReport>
    {
        Task<Int32> GetLastestCreatedReportIdAsync();

        Task<ViolationReport> GetLastestCreatedReportAsync();

        Task<IEnumerable<ViolationReport>> QueryAsync
            (ViolationReportQuery query, bool trackChanges = false);

        Task<ViolationReport> GetReportByIdAsync(int id);
    }
}
