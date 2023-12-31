using LostAndFound.Infrastructure.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IDashboardService
    {
        public Task<DashboardReadDTO> GetDashboardDataByMonthAsync(int month, int year);
    }
}
