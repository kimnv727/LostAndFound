using System;
using LostAndFound.Infrastructure.DTOs.User;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostReadDTO
    {
        public int Id { get; set; }
        public string PostUserId { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public string LocationLocationName { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserReadDTO User { get; set; }
    }
}