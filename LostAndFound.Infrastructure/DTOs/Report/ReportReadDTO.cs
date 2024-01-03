using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.ReportMedia;
using LostAndFound.Infrastructure.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Report
{
    public class ReportReadDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
        public string UserId { get; set; }
        public int ItemId { get; set; }
        public DateTime CreatedDate { get; set; }
        public ReportStatus Status { get; set; }
        public UserReadDTO User { get; set; }
        public ItemReadDTO Item { get; set; }
        public ICollection<ReportMediaReadDTO> ReportMedias { get; set; }

        

    }
}
