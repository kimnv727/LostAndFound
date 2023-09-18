using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Property;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IPropertyRepository : 
        IAddAsync<Property>,
        IGetAllAsync<Property>,
        IUpdate<Property>,
        IFindAsync<Property>,
        IDelete<Property>
    {
        public Task<IEnumerable<Property>> QueryPropertyAsync(PropertyQuery query, bool trackChanges = false);
        public Task<IEnumerable<Property>> QueryPropertyIgnoreStatusAsync(PropertyQuery query, bool trackChanges = false);

        public Task<Property> FindPropertyByIdAsync(int propertyId);
        public Task<Property> FindPropertyByNameAsync(string propertyName);
    }
}