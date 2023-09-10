using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.Post;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IPostService
    {
        Task UpdatePostStatusAsync(int postId, PostStatus postStatus);
        Task DeletePostAsync(int postId);
        Task<PostDetailWithCommentsReadDTO> GetPostByIdAsync(int postId);
        Task<PaginatedResponse<PostReadDTO>> GetPostByUserIdAsync(string userId);
        Task<PaginatedResponse<PostReadDTO>> QueryPostAsync(PostQuery query);
        Task<PaginatedResponse<PostDetailReadDTO>> QueryPostWithStatusAsync(PostQueryWithStatus query);
        Task<PostDetailReadDTO> CreatePostAsync(string userId, PostWriteDTO postWriteDTO);
        Task<PostDetailReadDTO> UpdatePostDetailsAsync(int postId, PostUpdateDTO postUpdateDTO);
        
        //TODO: GetPost include all comment
    }
}