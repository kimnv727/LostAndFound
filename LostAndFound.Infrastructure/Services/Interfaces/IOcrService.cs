using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IOcrService
    {
        public Task<string> GetEngOcrFromImageUrlAsync(string imageUrl);
        public Task<string> GetVieOcrFromImageUrlAsync(string imageUrl);
    }
}
