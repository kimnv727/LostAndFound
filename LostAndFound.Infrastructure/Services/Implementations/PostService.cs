using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.Services.Interfaces;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class PostService : IPostService
    {
        public Task UpdatePostStatusAsync(int postId)
        {
            throw new System.NotImplementedException();
        }

        public Task DeletePostAsync(int postId)
        {
            throw new System.NotImplementedException();
        }

        public Task<PostDetailReadDTO> GetPostByIdAsync(int postId)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedResponse<PostReadDTO>> QueryPostAsync(PostQuery query)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedResponse<PostReadDTO>> QueryPostIgnoreStatusAsync(ItemQuery query)
        {
            throw new System.NotImplementedException();
        }

        public Task<PostDetailReadDTO> CreatePostAsync(PostWriteDTO postWriteDTO)
        {
            throw new System.NotImplementedException();
        }

        public Task<PostDetailReadDTO> UpdatePostDetailsAsync(int postId, PostUpdateDTO postUpdateDTO)
        {
            throw new System.NotImplementedException();
        }
    }
}