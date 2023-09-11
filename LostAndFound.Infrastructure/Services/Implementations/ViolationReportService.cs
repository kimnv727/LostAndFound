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

        public async Task<ViolationReportReadDTO> CreateReportAsync(CreateReportDTO report, string userId)
        {
            try
            {
                if (userId.Equals(report.ReportedUserId))
                    throw new CreateReportException();

                var r = _mapper.Map<ViolationReport>(report.ViolationReport);
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
                return _mapper.Map<ViolationReportReadDTO>
                    (await _violationReportRepository.GetReportByIdAsync(reportId));
            } catch (Exception ex)
            {
                _violationReportRepository.Delete(
                    await _violationReportRepository.GetLastestCreatedReportAsync());
                await _unitOfWork.CommitAsync();
                throw new CreateReportException();
            }
        }

        public async Task<PaginatedResponse<ViolationReportReadDTO>> QueryViolationReport
            (ViolationReportQuery query)
        {
            return PaginatedResponse<ViolationReportReadDTO>
                .FromEnumerableWithMapping(await _violationReportRepository.QueryAsync(query)
                , query, _mapper);
        }

        public async Task<ViolationReportReadDTO> GetReportById(int id)
        {
            var report = await _violationReportRepository.GetReportByIdAsync(id);

            if (report == null)
                throw new ViolationReportNotFoundException();
            var r = _mapper.Map<ViolationReportReadDTO>(report);
            return r;
        }
    }
}
