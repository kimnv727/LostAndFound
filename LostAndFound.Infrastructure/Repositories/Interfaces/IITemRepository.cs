using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IITemRepository : 
        IGetAllAsync<Item>,
        IDelete<Item>,
        IUpdate<Item>,
        IFindAsync<Item>
    {
        Task<IEnumerable<Item>> QueryItemAsync(ItemQuery query, bool trackChanges = false);
        Task<Item> FindItemByIdAsync(Guid itemId);
    }
}
