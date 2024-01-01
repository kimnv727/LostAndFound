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
                Transactions = new List<Transaction>(),
                LineDataItem = new List<DTOs.Dashboard.Data>(),
                LineDataPost = new List<DTOs.Dashboard.Data>(),
                PopularCategories = new List<PopularCategory>()
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
            if(receiptList.Count() > 0)
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
                readDTO.Transactions = transactions;
            }

            //Get Line Data for Item
            readDTO.LineDataItem = (List<DTOs.Dashboard.Data>)await _itemRepository.GetItemCountsInDateRanges(month, year);

            //Get Line Data for Post
            readDTO.LineDataPost = (List<DTOs.Dashboard.Data>)await _postRepository.GetPostCountsInDateRanges(month, year);

            //Get top 10 Popular Category of that month
            var categoryDictionary = await _postRepository.GetTop10CategoryEntryCountByMonth(month, year);
            if (categoryDictionary.Count > 0)
            {
                foreach (var c in categoryDictionary)
                {
                    readDTO.PopularCategories.Add(new PopularCategory
                    {
                        CategoryName = c.Key.Name,
                        GroupCategoryName = c.Key.CategoryGroup.Name,
                        Amount = c.Value.ToString()
                    });
                }
            }

            return readDTO;
        }
    }
}
