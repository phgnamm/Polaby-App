

using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class HealthRepository : GenericRepository<Health>, IHealthRepository
    {
        private readonly AppDbContext _dbContext;

        public HealthRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }
    }
}
