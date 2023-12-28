using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Property;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface ICampusService
    {
        public Task<PaginatedResponse<CampusReadDTO>> QueryCampusAsync(CampusQuery query);
        public Task<IEnumerable<CampusReadDTO>> ListAllWithLocationsAsync();
        //public Task<IEnumerable<CampusReadDTO>> ListWithLocationsByCampusLocationAsync(CampusLocation campusLocation);
        public Task<CampusReadDTO> GetCampusByIdAsync(int CampusId);
        public Task<CampusReadDTO> CreateCampusAsync(string userId, CampusWriteDTO CampusWriteDTO);
        public Task<CampusReadDTO> UpdateCampusDetailsAsync(int CampusId, CampusWriteDTO CampusWriteDTO);
        public Task<CampusReadDTO> ChangeCampusStatusAsync(int CampusId);
        public Task<CampusReadDTO> DeleteCampusAsync(int CampusId);
        
    }
}