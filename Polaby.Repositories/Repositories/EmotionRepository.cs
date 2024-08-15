using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Repositories
{
    public class EmotionRepository : GenericRepository<Emotion>, IEmotionRepository
    {
        public EmotionRepository(AppDbContext dbContext, IClaimsService claimsService)
         : base(dbContext, claimsService)
        {
        }
        public async Task<Emotion?> GetEmotionByDateAsync(Guid userId, DateOnly date)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.UserId == userId && e.Date == date);
        }
    }
}


