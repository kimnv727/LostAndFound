using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class UserDeviceRepository : GenericRepository<UserDevice>, IUserDeviceRepository
    {
        public UserDeviceRepository(LostAndFoundDbContext context) : base(context)
        {
        }
        
        public async Task<UserDevice> FindUserDeviceByTokenAsync(string fcmToken)
        {
            return await _context.UserDevices.FirstOrDefaultAsync(ud => ud.Token == fcmToken);
        }
        
        public async Task<IEnumerable<UserDevice>> FindUserDevicesOfUserAsync(string userId)
        {
            IQueryable<UserDevice> userDevices = _context.UserDevices.Where(ud => ud.UserId == userId);

            return await Task.FromResult(userDevices.ToList());
        }
    }
}