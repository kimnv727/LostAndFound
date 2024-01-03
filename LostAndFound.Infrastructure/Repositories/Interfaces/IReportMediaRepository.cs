using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IReportMediaRepository :
        IAddAsync<ReportMedia>,
        IGetAllAsync<ReportMedia>
    {
        Task<IEnumerable<ReportMedia>> FindReportMediaIncludeMediaAsync(int reportId);
    }
}
