using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Cabinet;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class CabinetRepository : GenericRepository<Cabinet>, ICabinetRepository
    {
        public CabinetRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Cabinet>> FindAllCabinetsByStorageIdAsync(int storageId)
        {
            IQueryable<Cabinet> cabinets = _context.Cabinets
                .Include(c => c.Items)
                .Include(c => c.Storage)
                .ThenInclude(s => s.Campus)
                .Where(c => c.StorageId == storageId && c.IsActive == true).OrderBy(c => c.Name);

            return await Task.FromResult(cabinets.ToList());
        }

        public async Task<IEnumerable<Cabinet>> FindAllCabinetsByStorageIdIgnoreStatusAsync(int storageId)
        {
            IQueryable<Cabinet> cabinets = _context.Cabinets
                .Include(c => c.Items)
                .Include(c => c.Storage)
                .ThenInclude(s => s.Campus)
                .Where(c => c.StorageId == storageId).OrderBy(c => c.Name);

            return await Task.FromResult(cabinets.ToList());
        }

        public async Task<Cabinet> FindCabinetByIdAsync(int id)
        {
            return await _context.Cabinets
                .Include(c => c.Items)
                .Include(c => c.Storage)
                .ThenInclude(s => s.Campus)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive == true);
        }

        public async Task<Cabinet> FindCabinetByIdIgnoreStatusAsync(int id)
        {
            return await _context.Cabinets
                .Include(c => c.Items)
                .Include(c => c.Storage)
                .ThenInclude(s => s.Campus)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Cabinet>> ListAllCabinetsAsync()
        {
            IQueryable<Cabinet> cabinets = _context.Cabinets
                .Include(c => c.Items)
                .Include(c => c.Storage)
                .ThenInclude(s => s.Campus)
                .Where(c => c.IsActive == true)
                .OrderBy(c => c.Name);

            return await Task.FromResult(cabinets.ToList());
        }

        public async Task<IEnumerable<Cabinet>> QueryCabinetAsync(CabinetQuery query, bool trackChanges = false)
        {
            IQueryable<Cabinet> cabinets = _context.Cabinets
                .Include(c => c.Items)
                .Include(c => c.Storage)
                .ThenInclude(s => s.Campus)
                .AsSplitQuery();

            if (!trackChanges)
            {
                cabinets = cabinets.AsTracking();
            }

            if (query.Id > 0)
            {
                cabinets = cabinets.Where(c => c.Id == query.Id);
            }

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                cabinets = cabinets.Where(s => s.Name.ToLower().Contains(query.Name.ToLower()));
            }

            if (query.StorageId > 0)
            {
                cabinets = cabinets.Where(c => c.StorageId == query.StorageId);
            }

            if (query.CampusId > 0)
            {
                cabinets = cabinets.Where(c => c.Storage.CampusId == query.CampusId);
            }

            if (Enum.IsDefined(query.IsActive))
            {
                switch (query.IsActive)
                {
                    case CabinetQuery.ActiveStatus.Active:
                        cabinets = cabinets.Where(s => s.IsActive == true);
                        break;
                    case CabinetQuery.ActiveStatus.Disabled:
                        cabinets = cabinets.Where(s => s.IsActive == false);
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                cabinets = cabinets.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(cabinets.ToList());
        }
    }
}
