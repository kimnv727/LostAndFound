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
    public class ViolationReportRepository : GenericRepository<ViolationReport>, IViolationReportRepository
    {
        public ViolationReportRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<Int32> GetLastestCreatedReportIdAsync()
        {
            return await _context.ViolationReports.CountAsync();
        }
    }
}
