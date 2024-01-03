using LostAndFound.Infrastructure.DTOs.ReportMedia;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IReportMediaService
    {
        public Task<IEnumerable<ReportMediaReadDTO>> GetReportMedias(int reportId);
        public Task<IEnumerable<ReportMediaReadDTO>> UploadReportMedias(string userId, int reportId, IFormFile[] files);
        public Task DeleteReportMedia(string userId, int reportId, Guid mediaId);
    }
}
