using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Property;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class CampusRepository : GenericRepository<Campus>, ICampusRepository
    {
        public CampusRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Campus>> QueryCampusAsync(CampusQuery query, bool trackChanges = false)
        {
            IQueryable<Campus> campuses = _context.Campuses
                .Include(p => p.Locations)
                .AsSplitQuery();

            if (!trackChanges)
            {
                campuses = campuses.AsNoTracking();
            }
            
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                campuses = campuses.Where(p => p.Name.ToLower().Contains(query.Name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(query.Address))
            {
                campuses = campuses.Where(p => p.Address.ToLower().Contains(query.Address.ToLower()));
            }
            switch (query.IsActive)
            {
                case true:
                    campuses = campuses.Where(p => p.IsActive == true);
                    break;
                case false:
                    campuses = campuses.Where(p => p.IsActive == false);
                    break;
            }

            return await Task.FromResult(campuses.ToList());
        }

        public async Task<IEnumerable<Campus>> QueryCampusIgnoreStatusAsync(CampusQuery query, bool trackChanges = false)
        {
            IQueryable<Campus> campuses = _context.Campuses
                .Include(p => p.Locations)
                .Where(p => p.IsActive == true)
                .AsSplitQuery();

            if (!trackChanges)
            {
                campuses = campuses.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                campuses = campuses.Where(p => p.Name.ToLower().Contains(query.Name.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(query.Address))
            {
                campuses = campuses.Where(p => p.Address.ToLower().Contains(query.Address.ToLower()));
            }

            return await Task.FromResult(campuses.ToList());
        }

        public async Task<Campus> FindCampusByIdAsync(int CampusId)
        {
            return await _context.Campuses
                .Include(p => p.Locations)
                .FirstOrDefaultAsync(p => p.Id == CampusId);
        }

        public async Task<Campus> FindCampusByNameAsync(string PropertyName)
        {
            return await _context.Campuses
                .Include(p => p.Locations)
                .FirstOrDefaultAsync(p => p.Name.ToLower().Contains(PropertyName.ToLower()));
        }
    }
}