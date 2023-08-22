using LostAndFound.Core.Entities.Common;
using System.Linq;

namespace LostAndFound.Infrastructure.Repositories.QueryExtension
{
    public static class SoftDeleteEntityQueryExtensions
    {
        public static IQueryable<T> ExcludeDeleted<T>(this IQueryable<T> query)
            where T : ISoftDeleteEntity
        {
            return query.Where(e => e.DeletedDate == null);
        }
    }
}
