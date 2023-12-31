using AutoMapper;
using LostAndFound.Infrastructure.DTOs.Dashboard;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IGiveawayRepository _giveawayRepository;

        public DashboardService(IMapper mapper, IUnitOfWork unitOfWork, IItemRepository itemRepository, IUserRepository userRepository,
            IPostRepository postRepository, ICategoryRepository categoryRepository, IGiveawayRepository giveawayRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _giveawayRepository = giveawayRepository;
        }

        public async Task<DashboardReadDTO> GetDashboardDataByMonthAsync(DateTime date)
        {
            //Initilize ReadDTO
            var readDTO = new DashboardReadDTO();

            //Get Item Found

            //Get Array of Dates
            int month = date.Month;
            int year = date.Year;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            DateTime[] arrayOfDates = new DateTime[daysInMonth];

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime newDate = new DateTime(year, month, day);
                arrayOfDates[day - 1] = newDate;
            }

            return null;
        }
    }
}
