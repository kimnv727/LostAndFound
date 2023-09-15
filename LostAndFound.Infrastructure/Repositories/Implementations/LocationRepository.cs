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
            return await _context.Locations.FirstOrDefaultAsync(l => l.Id == locationId);
        }

        public Task<Location> FindLocationByNameAsync(string Name)
        {
            return _context.Locations.FirstOrDefaultAsync
                (l => l.LocationName == Name);
        }

        public async Task<IEnumerable<Location>> QueryLocationsAsync(LocationQuery query, bool trackChanges = false)
        {
            
            IQueryable<Location> locations = _context.Locations.AsSplitQuery();

            if (!trackChanges)
            {
                locations = locations.AsNoTracking();
            }
            
            return await Task.FromResult(locations.ToList());
        }
    }
}