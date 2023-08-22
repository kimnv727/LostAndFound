using System.Collections.Generic;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces.Common
{
    public interface IQueryAsync<TEntity, TQuery>
    {
        Task<IEnumerable<TEntity>> QueryAsync(TQuery query, bool trackChanges = false);
    }
}
