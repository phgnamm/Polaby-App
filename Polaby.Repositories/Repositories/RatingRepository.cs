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
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        public RatingRepository(AppDbContext dbContext, IClaimsService claimsService)
         : base(dbContext, claimsService)
        {
        }

        public async Task<Rating?> GetByUserAndExpertIdAsync(Guid userId, Guid expertId)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.UserId == userId && r.ExpertId == expertId);
        }
    }
}
