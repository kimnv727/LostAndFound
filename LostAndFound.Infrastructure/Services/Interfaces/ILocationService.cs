using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Location;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface ILocationService 
    {
        public Task<PaginatedResponse<LocationReadDTO>> QueryLocationWithPaginationAsync(LocationQuery query);
        public Task<LocationReadDTO> ChangeLocationStatusAsync(int id);
        public Task<IEnumerable<LocationReadDTO>> QueryLocationAsync(LocationQuery query);
        public Task<IEnumerable<LocationReadDTO>> ListAllAsync();
        public Task DeleteLocationAsync(int LocationId);
        public Task<LocationReadDTO> FindLocationByIdAsync(int LocationId);
        public Task<LocationReadDTO> FindLocationByNameAsync(string locationName);
        public Task<LocationReadDTO> UpdateLocationDetailsAsync(int LocationId, LocationWriteDTO LocationWriteDTO);
        public Task<LocationReadDTO> CreateItemAsync(LocationWriteDTO locationWriteDTO);
    }
}