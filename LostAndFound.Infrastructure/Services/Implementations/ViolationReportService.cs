using AutoMapper;
using LostAndFound.Core.Entities;
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
    public class ViolationReportService : IViolationReportService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IViolationReportRepository _violationReportRepository;
        private readonly IUserViolationReportRepository _userViolationReportRepository;

        public ViolationReportService(IMapper mapper, IUnitOfWork unitOfWork, 
            IViolationReportRepository violationReportRepository, 
            IUserViolationReportRepository userViolationReportRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _violationReportRepository = violationReportRepository;
            _userViolationReportRepository = userViolationReportRepository;
        }

        public async Task CreateReportAsync(CreateReportDTO report, Guid userId)
        {
            var r = _mapper.Map<ViolationReport>(report.WriteDTO);
            r.Status = Core.Enums.ViolationStatus.PENDING;

            await _violationReportRepository.AddAsync(r);
            await _unitOfWork.CommitAsync();

            var reportId = await _violationReportRepository.GetLastestCreatedReportIdAsync();

            await _userViolationReportRepository.AddAsync(
                new UserViolationReport 
                { 
                    UserId = userId, 
                    ReportId = reportId,
                    Type = Core.Enums.ViolationType.SENT,
                });
            await _userViolationReportRepository.AddAsync(
                new UserViolationReport
                {
                    UserId = report.ReportedUserId,
                    ReportId = reportId,
                    Type = Core.Enums.ViolationType.RECEIVED,
                });
            await _unitOfWork.CommitAsync();
        }
    }
}
