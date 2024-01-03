using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class ReportMediaRepository : GenericRepository<ReportMedia>, IReportMediaRepository
    {
        public ReportMediaRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ReportMedia>> FindReportMediaIncludeMediaAsync(int reportId)
        {
            return await _context.ReportMedias
                .Where(rm => rm.Media.IsActive == true && rm.Media.DeletedDate == null)
                .Include(rm => rm.Report)
                .Include(rm => rm.Media)
                .Where(rm => rm.ReportId == reportId)
                .ToListAsync();
        }

    }
}
