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
    public class FollowRepository : GenericRepository<Follow>, IFollowRepository
    {
        private readonly AppDbContext _dbContext;

        public FollowRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<Follow> GetByUserAndExpert(Guid userId, Guid expertId)
        {
            return await _dbContext.Follow
                                   .FirstOrDefaultAsync(x => x.UserId == userId && x.ExpertId == expertId);
        }
    }
}
