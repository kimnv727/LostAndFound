using LostAndFound.Infrastructure.DTOs.Cabinet;
using LostAndFound.Infrastructure.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface ICabinetService
    {
        Task UpdateCabinetStatusAsync(int cabinetId);
        Task<CabinetReadDTO> GetCabinetByIdAsync(int id);
        Task<CabinetReadDTO> GetCabinetByIdIgnoreStatusAsync(int id);
        Task<IEnumerable<CabinetReadDTO>> GetAllCabinetsByStorageIdAsync(int storageId);
        Task<IEnumerable<CabinetReadDTO>> GetAllCabinetsByStorageIdIgnoreStatusAsync(int storageId);
        Task<PaginatedResponse<CabinetReadDTO>> QueryCabinetAsync(CabinetQuery query);
        Task<CabinetReadDTO> CreateCabinetAsync(CabinetWriteDTO cabinetWriteDTO);
        Task<CabinetReadDTO> UpdateCabinetDetailsAsync(int cabinetId, CabinetUpdateDTO cabinetUpdateDTO);
    }
}
