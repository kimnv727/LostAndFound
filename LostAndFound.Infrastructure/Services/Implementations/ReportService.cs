using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.ViolationReport;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.ViolationReport;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReportRepository _violationReportRepository;
        private readonly IUserViolationReportRepository _userViolationReportRepository;

        public ReportService(IMapper mapper, IUnitOfWork unitOfWork, 
            IReportRepository violationReportRepository, 
            IUserViolationReportRepository userViolationReportRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _violationReportRepository = violationReportRepository;
            _userViolationReportRepository = userViolationReportRepository;
        }

        public async Task<ReportReadDTO> CreateReportAsync(CreateReportDTO report, string userId)
        {
            try
            {
                if (userId.Equals(report.ReportedUserId))
                    throw new CreateReportException();

                var r = _mapper.Map<Report>(report.ViolationReport);
                r.Status = Core.Enums.ReportStatus.PENDING;

                await _violationReportRepository.AddAsync(r);
                await _unitOfWork.CommitAsync();

                var reportId = await _violationReportRepository.GetLastestCreatedReportIdAsync();

                await _userViolationReportRepository.AddAsync(
                    new UserReport
                    {
                        UserId = userId,
                        ReportId = reportId,
                        Type = Core.Enums.ReportType.SENT,
                    });
                await _userViolationReportRepository.AddAsync(
                    new UserReport
                    {
                        UserId = report.ReportedUserId,
                        ReportId = reportId,
                        Type = Core.Enums.ReportType.RECEIVED,
                    });
                await _unitOfWork.CommitAsync();
                return _mapper.Map<ReportReadDTO>
                    (await _violationReportRepository.GetReportByIdAsync(reportId));
            } catch (Exception ex)
            {
                _violationReportRepository.Delete(
                    await _violationReportRepository.GetLastestCreatedReportAsync());
                await _unitOfWork.CommitAsync();
                throw new CreateReportException();
            }
        }

        public async Task<PaginatedResponse<ReportReadDTO>> QueryViolationReport
            (ReportQuery query)
        {
            return PaginatedResponse<ReportReadDTO>
                .FromEnumerableWithMapping(await _violationReportRepository.QueryAsync(query)
                , query, _mapper);
        }

        public async Task<ReportReadDTO> GetReportById(int id)
        {
            var report = await _violationReportRepository.GetReportByIdAsync(id);

            if (report == null)
                throw new ReportNotFoundException();
            var r = _mapper.Map<ReportReadDTO>(report);
            return r;
        }
    }
}
