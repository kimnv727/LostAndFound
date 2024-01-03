using LostAndFound.Infrastructure.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Report
{
    public class ReportQuery : PaginatedQuery, IOrderedQuery
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public int ItemId { get; set; }
        public int CampusId { get; set; }
        public enum ReportStatusQuery
        {
            All,
            PENDING,
            RESOLVED
        }
        [DefaultValue(ReportStatusQuery.All)]
        public ReportStatusQuery ReportStatus { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string OrderBy { get; set; } = "CreatedDate Desc";
    }
}
