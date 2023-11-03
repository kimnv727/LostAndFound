using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Storage;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IStorageRepository :
        IGetAllAsync<Storage>,
        IAddAsync<Storage>,
        IUpdate<Storage>,
        IDelete<Storage>
    {
        Task<Storage> FindStorageByIdAsync(int id);
        Task<Storage> FindStorageByIdIgnoreStatusAsync(int id);
        Task<Storage> FindStorageByIdIncludeCabinetsAsync(int id);
        Task<Storage> FindStorageByIdIncludeCabinetsIgnoreStatusAsync(int id);
        Task<IEnumerable<Storage>> FindAllStoragesByCampusIdAsync(int campusId);
        Task<IEnumerable<Storage>> FindAllStoragesByCampusIdIgnoreStatusAsync(int campusId);
        Task<IEnumerable<Storage>> FindAllStoragesByCampusIdIncludeCabinetsAsync(int campusId);
        Task<IEnumerable<Storage>> FindAllStoragesByCampusIdIgnoreStatusIncludeCabinetsAsync(int campusId);
        Task<IEnumerable<Storage>> QueryStorageAsync(StorageQuery query, bool trackChanges = false);
    }
}
