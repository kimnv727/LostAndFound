using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Location;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface ILocationRepository : 
        IGetAllAsync<Location>,
        IDelete<Location>,
        IUpdate<Location>,
        IFindAsync<Location>,
        IAddAsync<Location>
    {
        public Task<Location> FindLocationByIdAsync(int locationId);
        public Task<Location> FindLocationByNameAsync(string locationName);
        public Task<IEnumerable<Location>> QueryLocationsAsync(LocationQuery query, bool trackChanges = false);
        public Task<IEnumerable<Location>> GetAllWithCampusAsync();
        
    }
}