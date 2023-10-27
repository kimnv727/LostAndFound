using LostAndFound.Infrastructure.DTOs.UserMedia;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IUserMediaService
    {
        Task<UserMediaReadDTO> UploadUserAvatar(IFormFile file, string userId);
        Task<ICollection<UserMediaReadDTO>> UploadUserCredentialForVerification(string userId, string schoolId, IFormFile ccid, IFormFile studentCard);
    }
}
