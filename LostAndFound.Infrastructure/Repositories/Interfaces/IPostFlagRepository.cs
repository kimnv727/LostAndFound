using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IPostFlagRepository :
        IGetAllAsync<PostFlag>,
        IAddAsync<PostFlag>,
        IUpdate<PostFlag>,
        IDelete<PostFlag>
    {
        Task<int> CountPostFlagAsync(int postId);
        Task<PostFlag> FindPostFlagAsync(int postId, string userId);
        Task<IEnumerable<Post>> FindPostFlagsByUserIdAsync(string userId);
    }
}