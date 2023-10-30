using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Property;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface ICampusService
    {
        public Task<PaginatedResponse<CampusReadDTO>> QueryCampusAsync(CampusQuery query);
        public Task<IEnumerable<CampusReadDTO>> ListAllAsync();
        public Task<CampusReadDTO> GetCampusByIdAsync(int CampusId);
        public Task<CampusReadDTO> CreateCampusAsync(string userId, CampusWriteDTO CampusWriteDTO);
        public Task<CampusReadDTO> UpdateCampusDetailsAsync(int CampusId, CampusWriteDTO CampusWriteDTO);
        public Task ChangeCampusStatusAsync(int CampusId);
        public Task DeleteCampusAsync(int CampusId);
        
    }
}