using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.DTOs.PostFlag;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IPostFlagService
    {
        public Task<int> CountPostFlagAsync(int postId);
        public Task<PostFlagReadDTO> GetPostFlag(string userId, int postId);
        public Task<IEnumerable<PostReadDTO>> GetOwnPostFlags(string userId);
        public Task<PostFlagReadDTO> FlagAPost(string userId, int postId, PostFlagReason reason);
    }
}