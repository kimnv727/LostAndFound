using LostAndFound.Infrastructure.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Comment
{
    public class CommentQueryWithFlag : PaginatedQuery, IOrderedQuery
    {
        public string CommentUserId { get; set; }
        public int PostId { get; set; }
        public string CommentContent { get; set; }
        public string CommentPath { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int FlagCount { get; set; }
        public string OrderBy { get; set; } = "CreatedDate Desc";
    }
}
