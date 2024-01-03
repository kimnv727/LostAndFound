using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Authenticate;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.DTOs.ReportMedia;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class ReportMediaService : IReportMediaService
    {
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AwsCredentials _awsCredentials;
        private readonly IReportMediaRepository _reportMediaRepository;
        private readonly IReportRepository _reportRepository;

        public ReportMediaService(IMapper mapper, IMediaService mediaService, IUnitOfWork unitOfWork, AwsCredentials awsCredentials, 
            IReportRepository reportRepository, IReportMediaRepository reportMediaRepository)
        {
            _mapper = mapper;
            _mediaService = mediaService;
            _unitOfWork = unitOfWork;
            _awsCredentials = awsCredentials;
            _reportRepository = reportRepository;
            _reportMediaRepository = reportMediaRepository;
        }

        public async Task DeleteReportMedia(string userId, int reportId, Guid mediaId)
        {
            var report = await _reportRepository.GetReportByIdAsync(reportId);
            if (report == null)
            {
                throw new EntityWithAttributeNotFoundException<Report>(nameof(Report.Id), reportId);
            }

            if (report.UserId != userId)
            {
                throw new UserNotPermittedToResourceException<Report>();
            }

            var media = await _mediaService.FindMediaById(mediaId);
            if (media == null)
            {
                throw new EntityWithAttributeNotFoundException<Media>(nameof(Media.Id), mediaId);
            }

            await _mediaService.DeleteMediaAsync(mediaId);
        }

        public async Task<IEnumerable<ReportMediaReadDTO>> GetReportMedias(int reportId)
        {
            var report = await _reportRepository.GetReportByIdAsync(reportId);
            if (report == null)
            {
                throw new EntityWithAttributeNotFoundException<Report>(nameof(Report.Id), reportId);
            }

            IEnumerable<ReportMedia> rm = await _reportMediaRepository.FindReportMediaIncludeMediaAsync(reportId);
            return _mapper.Map<List<ReportMediaReadDTO>>(rm.ToList());
        }

        public async Task<IEnumerable<ReportMediaReadDTO>> UploadReportMedias(string userId, int reportId, IFormFile[] files)
        {
            var report = await _reportRepository.GetReportByIdAsync(reportId);
            if (report == null)
            {
                throw new EntityWithAttributeNotFoundException<Report>(nameof(Report.Id), reportId);
            }

            if (report.UserId != userId)
            {
                throw new UserNotPermittedToResourceException<Report>();
            }

            List<S3ReponseDTO> uploadResult = new List<S3ReponseDTO>();
            string[] filename = files.Select(f => f.FileName).ToArray();

            for (int i = 0; i < files.Length; i++)
            {
                var upload = await _mediaService.UploadFileAsync(files[i], _awsCredentials);
                uploadResult.Add(upload);
            }
            await _unitOfWork.CommitAsync();

            List<ReportMedia> reportMedias = new List<ReportMedia>();
            int j = 0;
            foreach (var ul in uploadResult)
            {

                ReportMedia rm = new ReportMedia()
                {
                    ReportId = reportId,
                    Media = new Media()
                    {
                        Name = filename[j],
                        Description = "Image from report with id " + reportId,
                        URL = ul.Url
                    }
                };
                j++;
                await _reportMediaRepository.AddAsync(rm);
                reportMedias.Add(rm);
            }
            await _unitOfWork.CommitAsync();
            return _mapper.Map<IEnumerable<ReportMediaReadDTO>>(reportMedias);
        }
    }
}

