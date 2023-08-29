using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class MediaRepository : GenericRepository<Media>, IMediaRepository
    {
        public MediaRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Media>> QueryMediaAsync(MediaQuery query, bool trackChanges = false)
        {
            IQueryable<Media> medias = _context.Medias.Where(m => m.IsActive == true).AsSplitQuery();

            if (!trackChanges)
            {
                medias = medias.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                medias = medias.Where(r => r.Name.ToLower().Contains(query.Name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                medias = medias.Where(r => r.Description.ToLower().Contains(query.Description.ToLower()));
            }

            return await Task.FromResult(medias.ToList());
        }

        public async Task<IEnumerable<Media>> QueryMediaIgnoreStatusAsync(MediaQueryWithStatus query, bool trackChanges = false)
        {
            IQueryable<Media> medias = _context.Medias.AsSplitQuery();

            if (!trackChanges)
            {
                medias = medias.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                medias = medias.Where(r => r.Name.ToLower().Contains(query.Name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                medias = medias.Where(r => r.Description.ToLower().Contains(query.Description.ToLower()));
            }
            if (query.IsActive != null)
            {
                if (query.IsActive == true)
                {
                    medias = medias.Where(r => r.IsActive == true);
                }
                if (query.IsActive == false)
                {
                    medias = medias.Where(r => r.IsActive == false);
                }
            }

            return await Task.FromResult(medias.ToList());
        }

        public async Task<Media> FindMediaByIdAsync(Guid mediaId)
        {
            return await _context.Medias
                .Where(m => m.IsActive == true && m.DeletedDate == null)
                .FirstOrDefaultAsync(m => m.Id == mediaId);
        }
    }
}
