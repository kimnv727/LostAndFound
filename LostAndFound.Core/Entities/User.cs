using LostAndFound.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Core.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FullName { get; set; }

        public Gender Gender { get; set; }

        public string? Avatar { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Password { get; set; }

        //TODO: Check to see if address for account is needed or not
        //public Guid AddressId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
