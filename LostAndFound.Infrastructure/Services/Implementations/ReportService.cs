using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Core.Exceptions.Item;
using LostAndFound.Core.Exceptions.ViolationReport;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Report;
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
        private readonly IReportRepository _reportRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IReportMediaService _reportMediaService;

        public ReportService(IMapper mapper, IUnitOfWork unitOfWork, IReportRepository reportRepository,
            IUserRepository userRepository, IItemRepository itemRepository, IReportMediaService reportMediaService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _reportRepository = reportRepository;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _reportMediaService = reportMediaService;
        }

        public async Task<ReportReadDTO> CreateReportAsync(string userId, ReportWriteDTO writeDTO)
        {
            //Get User
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }

            //Get number of Report
            var todayReport = await _reportRepository.CountTodayReportByUserIdAsync(userId);
            if (todayReport.Count() > 3)
            {
                throw new CreateReportPastLimitException();
            }
            var itemReport = await _reportRepository.GetReportByUserAndItemIdAsync(userId, writeDTO.ItemId);
            if (itemReport.Count() > 3)
            {
                throw new CreateReportPastLimitForThisItemException();
            }

            //Get Item
            var item = await _itemRepository.FindItemByIdAsync(writeDTO.ItemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(writeDTO.ItemId);
            }
            if(item.ItemStatus != Core.Enums.ItemStatus.RETURNED)
            {
                throw new ItemNotYetReturnedException();
            }

            //Map Report
            var report = _mapper.Map<Report>(writeDTO);
            report.Status = Core.Enums.ReportStatus.PENDING;
            report.UserId = userId;
            //Add Report
            await _reportRepository.AddAsync(report);
            await _unitOfWork.CommitAsync();

            //AddMedia
            if (writeDTO.Medias != null)
            {
                await _reportMediaService.UploadReportMedias(userId, report.Id, writeDTO.Medias);
                await _unitOfWork.CommitAsync();
            }
            var result = _mapper.Map<ReportReadDTO>(report);

            return result;
        }

        public async Task<ReportReadDTO> UpdateReportStatusAsync(int reportId, ReportStatus reportStatus)
        {
            var report = await _reportRepository.GetReportByIdAsync(reportId);
            if (report == null)
            {
                throw new EntityWithIDNotFoundException<Report>(reportId);
            }
            report.Status = reportStatus;
            await _unitOfWork.CommitAsync();

            return _mapper.Map<ReportReadDTO>(report);
        }

        public async Task<PaginatedResponse<ReportReadDTO>> QueryReports(ReportQuery query)
        {
            var reports = await _reportRepository.QueryAsync(query);

            return PaginatedResponse<ReportReadDTO>.FromEnumerableWithMapping(reports, query, _mapper);
        }

        public async Task<ReportReadDTO> GetReportById(int reportId)
        {
            var report = await _reportRepository.GetReportByIdAsync(reportId);

            if (report == null)
            {
                throw new EntityWithIDNotFoundException<Report>(reportId);
            }

            return _mapper.Map<ReportReadDTO>(report);
        }

        public async Task<PaginatedResponse<ReportReadDTO>> GetReportByUserAndItemId(string userId, int itemId)
        {
            //Get User
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }

            //Get Item
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }
            //Get Reports
            var reports = await _reportRepository.GetReportByUserAndItemIdAsync(userId, itemId);

            return _mapper.Map<PaginatedResponse<ReportReadDTO>>(reports);
        }

        public async Task<PaginatedResponse<ReportReadDTO>> GetReportByUserId(string userId)
        {
            //Get User
            var user = await _userRepository.FindUserByID(userId);
            if (user == null)
            {
                throw new EntityWithIDNotFoundException<User>(userId);
            }
            //Get Reports
            var reports = await _reportRepository.GetReportsByUserIdAsync(userId);

            return _mapper.Map<PaginatedResponse<ReportReadDTO>>(reports);
        }

        public async Task<PaginatedResponse<ReportReadDTO>> GetReportByItemId(int itemId)
        {
            //Get Item
            var item = await _itemRepository.FindItemByIdAsync(itemId);
            if (item == null)
            {
                throw new EntityWithIDNotFoundException<Item>(itemId);
            }
            //Get Reports
            var reports = await _reportRepository.GetReportsByItemIdAsync(itemId);

            return _mapper.Map<PaginatedResponse<ReportReadDTO>>(reports);
        }
    }
}
