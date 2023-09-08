using LostAndFound.Infrastructure.DTOs.UserMedia;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IUserMediaService
    {
        Task<UserMediaReadDTO> UploadUserAvatar(IFormFile file, string userId);
    }
}
