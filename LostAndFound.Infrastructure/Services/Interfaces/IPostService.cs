using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.Post;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IPostService
    {
        Task UpdatePostStatusAsync(int postId);
        Task DeletePostAsync(int postId);
        Task<PostDetailReadDTO> GetPostByIdAsync(int postId);
        Task<PaginatedResponse<PostReadDTO>> QueryPostAsync(PostQuery query);
        Task<PaginatedResponse<PostReadDTO>> QueryPostIgnoreStatusAsync(ItemQuery query);
        Task<PostDetailReadDTO> CreatePostAsync(PostWriteDTO postWriteDTO);
        Task<PostDetailReadDTO> UpdatePostDetailsAsync(int postId, PostUpdateDTO postUpdateDTO);
    }
}