using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Property;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface ICampusRepository : 
        IAddAsync<Campus>,
        IGetAllAsync<Campus>,
        IUpdate<Campus>,
        IFindAsync<Campus>,
        IDelete<Campus>
    {
        public Task<IEnumerable<Campus>> QueryCampusAsync(CampusQuery query, bool trackChanges = false);
        public Task<IEnumerable<Campus>> QueryCampusIgnoreStatusAsync(CampusQuery query, bool trackChanges = false);

        public Task<Campus> FindCampusByIdAsync(int CampusId);
        public Task<Campus> FindCampusByNameAsync(string CampusName);
    }
}