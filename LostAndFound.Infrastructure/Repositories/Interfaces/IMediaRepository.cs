using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IMediaRepository :
        IGetAllAsync<Media>,
        IAddAsync<Media>,
        IDelete<Media>,
        IUpdate<Media>,
        IFindAsync<Media>
    {
        Task<IEnumerable<Media>> QueryMediaAsync(MediaQuery query, bool trackChanges = false);
        Task<IEnumerable<Media>> QueryMediaIgnoreStatusAsync(MediaQueryWithStatus query, bool trackChanges = false);
        Task<Media> FindMediaByIdAsync(Guid mediaId);
        Task<Media> FindMediaByUrlAsync(string url);
    }
}
