using LostAndFound.Infrastructure.DTOs.ItemMedia;
using LostAndFound.Infrastructure.DTOs.UserMedia;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IItemMediaService
    {
        public Task<IEnumerable<ItemMediaReadDTO>> GetItemMedias(int itemId);
        public Task<IEnumerable<ItemMediaReadDTO>> UploadItemMedias(string userId, int itemId, IFormFile[] files);
        public Task DeleteItemMedia(string userId, int postId, Guid mediaId);
    }
}
