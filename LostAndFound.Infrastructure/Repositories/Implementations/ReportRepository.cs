using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Report;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Report>> QueryAsync (ReportQuery query, bool trackChanges = false)
        {
            IQueryable<Report> reports = _context.Reports
                .Include(r => r.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemClaims.Where(ic => ic.ClaimStatus == ClaimStatus.ACCEPTED))
                        .ThenInclude(ic => ic.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                        .ThenInclude(im => im.Media)
                .Include(r => r.ReportMedias)
                .ThenInclude(rm => rm.Media)
                .AsSplitQuery();

            if (!trackChanges)
            {
                reports = reports.AsNoTracking(); 
            }

            if (!string.IsNullOrWhiteSpace(query.UserId))
            {
                reports = reports.Where(r => r.UserId == query.UserId);
            }

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                reports = reports.Where(r => r.Title.ToLower().Contains(query.Title.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(query.Content))
            {
                reports = reports.Where(r => r.Content.ToLower().Contains(query.Content.ToLower()));
            }

            if (query.ItemId > 0)
            {
                reports = reports.Where(r => r.ItemId >= query.ItemId);
            }

            if (query.CampusId > 0)
            {
                reports = reports.Where(r => r.User.CampusId == query.CampusId);
            }

            if (Enum.IsDefined(query.ReportStatus))
            {
                if (query.ReportStatus == ReportQuery.ReportStatusQuery.PENDING)
                {
                    reports = reports.Where(r => r.Status == ReportStatus.PENDING);
                }
                else if (query.ReportStatus == ReportQuery.ReportStatusQuery.RESOLVED)
                {
                    reports = reports.Where(r => r.Status == ReportStatus.RESOLVED);
                }
                else if (query.ReportStatus == ReportQuery.ReportStatusQuery.REJECTED)
                {
                    reports = reports.Where(r => r.Status == ReportStatus.REJECTED);
                }
            }

            if (query.DateFrom != null)
            {
                reports = reports.Where(r => r.CreatedDate >= query.DateFrom);
            }
            if (query.DateTo != null)
            {
                reports = reports.Where(r => r.CreatedDate <= query.DateTo);
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                reports = reports.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(reports.ToList());
        }

        public async Task<Report> GetReportByIdAsync(int reportId)
        {
            return await _context.Reports
                .Include(r => r.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemClaims.Where(ic => ic.ClaimStatus == ClaimStatus.ACCEPTED))
                        .ThenInclude(ic => ic.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                        .ThenInclude(im => im.Media)
                .Include(r => r.ReportMedias)
                .ThenInclude(rm => rm.Media)
                .FirstOrDefaultAsync(r => r.Id == reportId);
        }

        public async Task<IEnumerable<Report>> CountTodayReportByUserIdAsync(string userId)
        {
            var result = _context.Reports
                .Where(r => r.UserId == userId && r.CreatedDate.Date == DateTime.Now.Date);
            return await Task.FromResult(result.ToList());
        }

        public async Task<IEnumerable<Report>> GetReportByUserAndItemIdAsync(string userId, int itemId)
        {
            var result = _context.Reports
                .Include(r => r.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemClaims.Where(ic => ic.ClaimStatus == ClaimStatus.ACCEPTED))
                        .ThenInclude(ic => ic.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                        .ThenInclude(im => im.Media)
                .Include(r => r.ReportMedias)
                .ThenInclude(rm => rm.Media)
                .Where(r => r.UserId == userId && r.ItemId == itemId);

            return await Task.FromResult(result.ToList());
        }

        public async Task<IEnumerable<Report>> GetReportsByUserIdAsync(string userId)
        {
            var result = _context.Reports
                .Include(r => r.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemClaims.Where(ic => ic.ClaimStatus == ClaimStatus.ACCEPTED))
                        .ThenInclude(ic => ic.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                        .ThenInclude(im => im.Media)
                .Include(r => r.ReportMedias)
                .ThenInclude(rm => rm.Media)
                .Where(r => r.UserId == userId);

            return await Task.FromResult(result.ToList());
        }

        public async Task<IEnumerable<Report>> GetReportsByItemIdAsync(int itemId)
        {
            var result = _context.Reports
                .Include(r => r.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemClaims.Where(ic => ic.ClaimStatus == ClaimStatus.ACCEPTED))
                        .ThenInclude(ic => ic.User)
                .Include(r => r.Item)
                    .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                        .ThenInclude(im => im.Media)
                .Include(r => r.ReportMedias)
                .ThenInclude(rm => rm.Media)
                .Where(r => r.ItemId == itemId);

            return await Task.FromResult(result.ToList());
        }
    }
}
