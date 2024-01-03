using LostAndFound.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Report
{
    public class ReportWriteDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int ItemId { get; set; }
        public IFormFile[] Medias { get; set; }
    }
}
