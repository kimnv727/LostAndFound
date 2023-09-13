using LostAndFound.Infrastructure.DTOs.PostMedia;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IPostMediaService
    {
        public Task<IEnumerable<PostMediaReadDTO>> GetPostMedias(int postId);
        public Task<IEnumerable<PostMediaReadDTO>> UploadPostMedias (string userId, int postId, IFormFile[] files);
        public Task DeletePostMedia(string userId, int postId, Guid mediaId);
    }
}
