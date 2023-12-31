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
        private readonly IReceiptRepository _receiptRepository;

        public DashboardService(IMapper mapper, IUnitOfWork unitOfWork, IItemRepository itemRepository, IUserRepository userRepository,
            IPostRepository postRepository, ICategoryRepository categoryRepository, IGiveawayRepository giveawayRepository,
            IReceiptRepository receiptRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _giveawayRepository = giveawayRepository;
            _receiptRepository = receiptRepository;
        }

        public async Task<DashboardReadDTO> GetDashboardDataByMonthAsync(int month, int year)
        {
            //Initilize ReadDTO
            var readDTO = new DashboardReadDTO() {
                ItemReturn = new ItemReturn(),
                Transactions = new Transaction[10],
                LineDataItem = new DTOs.Dashboard.Data[10],
                LineDataPost = new DTOs.Dashboard.Data[10],
                PopularCategories = new DTOs.Dashboard.PopularCategory[10]
            };

            //Get Item Found
            var newItemCreatedList = await _itemRepository.CountNewlyCreatedItem(month, year);
            readDTO.ItemFound = newItemCreatedList.Count();

            //Get Item Return 
            if(readDTO.ItemFound > 0)
            {
                readDTO.ItemReturn.Amount = newItemCreatedList.Where(i => i.ItemStatus == Core.Enums.ItemStatus.RETURNED).Count();
                readDTO.ItemReturn.Percentage = readDTO.ItemReturn.Amount / readDTO.ItemFound;
            }
            else
            {
                readDTO.ItemReturn.Amount = 0;
                readDTO.ItemReturn.Percentage = 0;
            }

            //Get Post Created
            var newPostCreatedList = await _postRepository.CountNewlyCreatedPost(month, year);
            readDTO.PostCreated = newPostCreatedList.Count();

            //Get new User
            var newUserList = await _userRepository.CountNewlyCreatedMember(month, year);
            readDTO.NewUsers = newUserList.Count();

            //Get Finished Giveaway
            var finishedGiveawayList = await _giveawayRepository.CountFinishedGiveaways(month, year);
            readDTO.FinishedGiveaways = finishedGiveawayList.Count();

            //Get Last 10 Item Return of specific month
            var receiptList = await _receiptRepository.GetLatestTenOfAMonthAsync(month, year);
            if(receiptList.Count() > 10)
            {
                var transactions = new List<Transaction>();
                foreach(var r in receiptList)
                {
                    //get user name
                    var user = await _userRepository.FindUserByID(r.ReceiverId);
                    var t = new DTOs.Dashboard.Transaction
                    {
                        ItemName = r.Item.Name,
                        ItemCategory = r.Item.Category.Name,
                        User = user.FullName,
                        ReturnDate = r.CreatedDate,
                    };
                    transactions.Add(t);
                }
                readDTO.Transactions = transactions.ToArray();
            }

            //Get Array of Dates
            int daysInMonth = DateTime.DaysInMonth(year, month);

            DateTime[] arrayOfDates = new DateTime[daysInMonth];

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime newDate = new DateTime(year, month, day);
                arrayOfDates[day - 1] = newDate;
            }

            return readDTO;
        }
    }
}
