using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class UserViolationReportRepository : GenericRepository<UserReport>, IUserViolationReportRepository
    {
        public UserViolationReportRepository(LostAndFoundDbContext context) : base(context)
        {
        }
    }
}
