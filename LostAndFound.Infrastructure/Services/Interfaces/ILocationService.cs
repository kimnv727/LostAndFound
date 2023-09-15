using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Location;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface ILocationService 
    {
        public Task<PaginatedResponse<LocationReadDTO>> QueryLocationAsync(LocationQuery query);
        public Task DeleteLocationAsync(int LocationId);
        public Task<LocationReadDTO> FindLocationByIdAsync(int LocationId);
        public Task<LocationReadDTO> UpdateLocationDetailsAsync(int LocationId, LocationWriteDTO LocationWriteDTO);
        
    }
}