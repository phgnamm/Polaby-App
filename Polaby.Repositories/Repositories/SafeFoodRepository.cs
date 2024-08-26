
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class SafeFoodRepository : GenericRepository<SafeFood>, ISafeFoodRepository
    {
        private readonly AppDbContext _dbContext;

        public SafeFoodRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }
    }
}
