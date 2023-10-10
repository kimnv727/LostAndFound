using LostAndFound.Infrastructure.DTOs.User;
using System;
using System.Collections.Generic;

namespace LostAndFound.Infrastructure.DTOs.Comment
{
    public class CommentDetailWithReplyDetailReadDTO
    {
        public int Id { get; set; }
        public string CommentUserId { get; set; }
        public int PostId { get; set; }
        public string CommentContent { get; set; }
        public string CommentPath { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<CommentReadDTO> Comments { get; set; }
        public UserReadDTO User { get; set; }

    }
}