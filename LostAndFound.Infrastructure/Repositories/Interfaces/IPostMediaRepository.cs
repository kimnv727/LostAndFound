using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IPostMediaRepository :
        IAddAsync<PostMedia>,
        IGetAllAsync<PostMedia>
    {
        Task<PostMedia> FindPostMediaIncludeMediaAsync(int postId);
    }
}
