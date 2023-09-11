using LostAndFound.Infrastructure.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.ViolationReport
{
    public class ViolationReportQuery : PaginatedQuery, ISearchTextQuery
    {
        public string SearchText { get; set; }//Search violation report title

        public ViolationQueryCategory Category { get; set; }

        public ViolationQueryStatus Status { get; set; }
    }

    public enum ViolationQueryStatus
    {
        ALL,
        PENDING,
        RESOLVED,
    }

    public enum ViolationQueryCategory
    {
        ALL,
        USER_VIOLATION,
        MANAGER_VIOLATION,
    }
}
