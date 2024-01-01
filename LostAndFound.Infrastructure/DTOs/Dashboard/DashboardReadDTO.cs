using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Dashboard
{
    public class DashboardReadDTO
    {
        public int ItemFound { get; set; }
        public ItemReturn ItemReturn { get; set; }
        public int PostCreated { get; set; }
        public int NewUsers { get; set; }
        //public int TotalUsers { get; set; }
        public int FinishedGiveaways { get; set; }
        public List<Data> LineDataItem { get; set; }
        public List<Data> LineDataPost { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<PopularCategory> PopularCategories { get; set; }
    }

    public class ItemReturn
    {
        public float Percentage { get; set; }
        public int Amount { get; set; }
    }

    public class Data
    {
        public string x { get; set; }
        public int y { get; set; }
    }

    public class Transaction
    {
        public string ItemName { get; set; }
        public string ItemCategory { get; set; }
        public string User { get; set; }
        public DateTime ReturnDate { get; set; }
    }

    public class PopularCategory
    {
        public string CategoryName { get; set; }
        public string GroupCategoryName { get; set; }
        public string Amount { get; set; }
    }
}
