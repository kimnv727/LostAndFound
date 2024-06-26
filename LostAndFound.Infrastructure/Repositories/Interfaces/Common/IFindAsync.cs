﻿using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces.Common
{
    public interface IFindAsync<T> where T : class
    {
        Task<T> FindAsync(params object[] keys);
    }
}
