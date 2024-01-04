using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Storage;
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
    public class StorageRepository : GenericRepository<Storage>, IStorageRepository
    {
        public StorageRepository(LostAndFoundDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Storage>> FindAllStoragesByCampusIdAsync(int campusId)
        {
            IQueryable<Storage> storages = _context.Storages
                .Include(s => s.Campus)
                .Include(s => s.User)
                .Where(s => s.CampusId == campusId && s.IsActive == true).OrderBy(s => s.Location);

            return await Task.FromResult(storages.ToList());
        }

        public async Task<IEnumerable<Storage>> FindAllStoragesByCampusIdIgnoreStatusAsync(int campusId)
        {
            IQueryable<Storage> storages = _context.Storages
                .Include(s => s.Campus)
                .Include(s => s.User)
                .Where(s => s.CampusId == campusId).OrderBy(s => s.Location);

            return await Task.FromResult(storages.ToList());
        }

        public async Task<IEnumerable<Storage>> FindAllStoragesByCampusIdIgnoreStatusIncludeCabinetsAsync(int campusId)
        {
            IQueryable<Storage> storages = _context.Storages
                .Where(s => s.CampusId == campusId)
                .Include(s => s.Campus)
                .Include(s => s.Cabinets)
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(s => s.Cabinets.Where(c => c.IsActive == true))
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.Category)
                .Include(s => s.Cabinets.Where(c => c.IsActive == true))
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.Location)
                .Include(s => s.User)
                .OrderBy(s => s.Location);

            return await Task.FromResult(storages.ToList());
        }

        public async Task<IEnumerable<Storage>> FindAllStoragesByCampusIdIncludeCabinetsAsync(int campusId)
        {
            IQueryable<Storage> storages = _context.Storages
                .Where(s => s.CampusId == campusId && s.IsActive == true)
                .Include(s => s.Campus)
                .Include(s => s.User)
                .Include(s => s.Cabinets.Where(c => c.IsActive == true))
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(s => s.Cabinets.Where(c => c.IsActive == true))
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.Category)
                .Include(s => s.Cabinets.Where(c => c.IsActive == true))
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.Location)
                .OrderBy(s => s.Location);

            return await Task.FromResult(storages.ToList());
        }

        public async Task<Storage> FindStorageByIdAsync(int id)
        {
            return await _context.Storages
                .Include(s => s.Campus)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive == true);
        }

        public async Task<Storage> FindStorageByIdIgnoreStatusAsync(int id)
        {
            return await _context.Storages
                .Include(s => s.Campus)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Storage> FindStorageByIdIncludeCabinetsAsync(int id)
        {
            return await _context.Storages
                .Include(s => s.Campus)
                .Include(s => s.User)
                .Include(s => s.Cabinets.Where(c => c.IsActive == true))
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(s => s.Cabinets.Where(c => c.IsActive == true))
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.Category)
                .Include(s => s.Cabinets.Where(c => c.IsActive == true))
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.Location)
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive == true);
        }

        public async Task<Storage> FindStorageByIdIncludeCabinetsIgnoreStatusAsync(int id)
        {
            return await _context.Storages
                .Include(s => s.Campus)
                .Include(s => s.Cabinets)
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(s => s.Cabinets.Where(c => c.IsActive == true))
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.Category)
                .Include(s => s.Cabinets.Where(c => c.IsActive == true))
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.Location)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Storage>> ListAllStoragesAsync()
        {
            IQueryable<Storage> storages = _context.Storages
                .Include(s => s.Campus)
                .Include(s => s.User)
                .Include(s => s.Cabinets)
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(s => s.Cabinets.Where(c => c.IsActive == true))
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.Category)
                .Include(s => s.Cabinets.Where(c => c.IsActive == true))
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.Location)
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.Location);

            return await Task.FromResult(storages.ToList());
        }

        public async Task<IEnumerable<Storage>> QueryStorageAsync(StorageQuery query, bool trackChanges = false)
        {
            IQueryable<Storage> storages = _context.Storages
                .Include(s => s.Campus)
                .Include(s => s.User)
                .Include(s => s.Cabinets.Where(c => c.IsActive == true))
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(s => s.Cabinets.Where(c => c.IsActive == true))
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.Category)
                .Include(s => s.Cabinets.Where(c => c.IsActive == true))
                .ThenInclude(c => c.Items)
                .ThenInclude(i => i.Location)
                .AsSplitQuery();

            if (!trackChanges)
            {
                storages = storages.AsTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.Location))
            {
                storages = storages.Where(s => s.Location.ToLower().Contains(query.Location.ToLower()));
            }

            if (query.Id > 0)
            {
                storages = storages.Where(s => s.Id == query.Id);
            }

            if (query.CampusId > 0)
            {
                storages = storages.Where(s => s.CampusId == query.CampusId);
            }

            if (Enum.IsDefined(query.IsActive))
            {
                switch (query.IsActive)
                {
                    case StorageQuery.ActiveStatus.Active:
                        storages = storages.Where(s => s.IsActive == true);
                        break;
                    case StorageQuery.ActiveStatus.Disabled:
                        storages = storages.Where(s => s.IsActive == false);
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(query.MainStorageManagerId))
            {
                storages = storages.Where(s => s.MainStorageManagerId == query.MainStorageManagerId);
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                storages = storages.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(storages.ToList());
        }
    }
}
