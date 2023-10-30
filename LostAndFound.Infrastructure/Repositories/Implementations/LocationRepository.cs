using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Location;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<Location> FindLocationByIdAsync(int locationId)
        {
            return await _context.Locations
                .Include(l => l.Property)
                .FirstOrDefaultAsync(l => l.Id == locationId);
        }

        public Task<Location> FindLocationByNameAsync(string Name)
        {
            return _context.Locations
                .Include(l => l.Property)
                .FirstOrDefaultAsync
                (l => l.LocationName == Name);
        }

        public async Task<IEnumerable<Location>> QueryLocationsAsync(LocationQuery query, bool trackChanges = false)
        {
            
            IQueryable<Location> locations = _context.Locations
                .Include(l => l.Property)
                .AsSplitQuery();

            if (!trackChanges)
            {
                locations = locations.AsNoTracking();
            }

            if(query.LocationId > 0)
            {
                locations = locations.Where(l => l.Id == query.LocationId);
            }
            if (query.PropertyId > 0)
            {
                locations = locations.Where(l => l.Property.Id == query.PropertyId);
            }
            if (!string.IsNullOrWhiteSpace(query.LocationName))
            {
                locations = locations.Where(l => l.LocationName.ToLower().Contains(query.LocationName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(query.PropertyName))
            {
                locations = locations.Where(l => l.Property.Name.ToLower().Contains(query.PropertyName.ToLower()));
            }
            if(query.Floor > 0)
            {
                locations = locations.Where(l => l.Floor == query.Floor);
            }

            return await Task.FromResult(locations.ToList());
        }
    }
}