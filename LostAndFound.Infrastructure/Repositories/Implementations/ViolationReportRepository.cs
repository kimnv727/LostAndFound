﻿using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.ViolationReport;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class ViolationReportRepository : GenericRepository<ViolationReport>, 
        IViolationReportRepository
    {
        public ViolationReportRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<ViolationReport> GetLastestCreatedReportAsync()
        {
            return await _context.ViolationReports.LastOrDefaultAsync();
        }

        public async Task<Int32> GetLastestCreatedReportIdAsync()
        {
            return await _context.ViolationReports.CountAsync();
        }

        public async Task<IEnumerable<ViolationReport>> QueryAsync
            (ViolationReportQuery query, bool trackChanges = false)
        {
            IQueryable<ViolationReport> violations = _context.ViolationReports
                .Include(vr => vr.UserViolationReports)
                .ThenInclude(uvr => uvr.User).AsSplitQuery();

            if (trackChanges)
                violations.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchText))
                violations = violations.Where(vr => vr.Title.Contains(query.SearchText));

            if (Enum.IsDefined(query.Category))
            {
                if (query.Category == ViolationQueryCategory.USER_VIOLATION)
                    violations = violations
                        .Where(vr => vr.Category == ViolationCategory.USER_VIOLATION);

                if (query.Category == ViolationQueryCategory.MANAGER_VIOLATION)
                    violations = violations
                        .Where(vr => vr.Category == ViolationCategory.MANAGER_VIOLATION);
            }

            if (Enum.IsDefined(query.Status))
            {
                if (query.Status == ViolationQueryStatus.PENDING)
                    violations = violations
                        .Where(vr => vr.Status == ViolationStatus.PENDING);

                if (query.Status == ViolationQueryStatus.RESOLVED)
                    violations = violations
                        .Where(vr => vr.Status == ViolationStatus.RESOLVED);
            }

            return await Task.FromResult(violations.ToList());
        }

        public async Task<ViolationReport> GetReportByIdAsync(int id)
        {
            return await _context.ViolationReports
                .Include(vr => vr.UserViolationReports)
                .ThenInclude(uvr => uvr.User).SingleOrDefaultAsync(vr => vr.Id == id);
        }
    }
}
