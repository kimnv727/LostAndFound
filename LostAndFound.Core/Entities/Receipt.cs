﻿using LostAndFound.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Entities
{
    public class Receipt
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        [ForeignKey("Item")]
        public string ItemId { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        public Guid ReceiptImage { get;set; }

        [Required]
        public ReceiptType ReceiptType { get; set; }

        public virtual Media Media {  get; set; }

    }
}