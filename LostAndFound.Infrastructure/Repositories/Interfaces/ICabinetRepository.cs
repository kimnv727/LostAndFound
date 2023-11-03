using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Cabinet;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface ICabinetRepository :
        IGetAllAsync<Cabinet>,
        IAddAsync<Cabinet>,
        IUpdate<Cabinet>,
        IDelete<Cabinet>
    {
        Task<Cabinet> FindCabinetByIdAsync(int id);
        Task<Cabinet> FindCabinetByIdIgnoreStatusAsync(int id);
        Task<IEnumerable<Cabinet>> FindAllCabinetsByStorageIdAsync(int storageId);
        Task<IEnumerable<Cabinet>> FindAllCabinetsByStorageIdIgnoreStatusAsync(int storageId);
        Task<IEnumerable<Cabinet>> QueryCabinetAsync(CabinetQuery query, bool trackChanges = false);
    }
}
