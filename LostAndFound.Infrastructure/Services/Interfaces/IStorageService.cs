using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IStorageService
    {
        Task UpdateStorageStatusAsync(int storageId);
        Task<StorageReadDTO> GetStorageByIdAsync(int id);
        Task<StorageReadDTO> GetStorageByIdIgnoreStatusAsync(int id);
        Task<StorageReadIncludeCabinetsDTO> GetStorageByIdIncludeCabinetsAsync(int id);
        Task<StorageReadIncludeCabinetsDTO> GetStorageByIdIncludeCabinetsIgnoreStatusAsync(int id);
        Task<IEnumerable<StorageReadDTO>> GetAllStoragesByCampusIdAsync(int campusId);
        Task<IEnumerable<StorageReadDTO>> GetAllStoragesByCampusIdIgnoreStatusAsync(int campusId);
        Task<IEnumerable<StorageReadIncludeCabinetsDTO>> GetAllStoragesByCampusIdIncludeCabinetsAsync(int campusId);
        Task<IEnumerable<StorageReadIncludeCabinetsDTO>> GetAllStoragesByCampusIdIgnoreStatusIncludeCabinetsAsync(int campusId);
        Task<PaginatedResponse<StorageReadIncludeCabinetsDTO>> QueryStorageAsync(StorageQuery query);
        Task<StorageReadDTO> CreateStorageAsync(StorageWriteDTO storageWriteDTO);
        Task<StorageReadDTO> UpdateStorageDetailsAsync(int storageId, StorageUpdateDTO storageUpdateDTO);
    }
}
