using Microsoft.AspNetCore.Http;
using LostAndFound.Infrastructure.DTOs.Media;
using System;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IMediaService
    {
        Task<S3ReponseDTO> UploadFileAsync(IFormFile file, AwsCredentials cred);
        Task UpdateMediaStatus(Guid mediaId);
        Task<MediaReadDTO> UpdateMediaDetail(Guid mediaId, MediaUpdateWriteDTO mediaUpdateWriteDTO);
        Task DeleteMediaAsync(Guid mediaId);
        Task<MediaReadDTO> FindMediaById(Guid mediaId);
        Task<DTOs.Common.PaginatedResponse<MediaReadDTO>> QueryMediaAsync(MediaQuery query);
        Task<DTOs.Common.PaginatedResponse<MediaDetailReadDTO>> QueryMediaIgnoreStatusAsync(MediaQueryWithStatus query);
    }
}
