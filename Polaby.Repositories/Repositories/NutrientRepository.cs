using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class NutrientRepository : GenericRepository<Nutrient>, INutrientRepository
    {
        private readonly AppDbContext _dbContext;
        public NutrientRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }
        public IQueryable<Nutrient> GetAll()
        {
            return _dbContext.Set<Nutrient>().AsQueryable();
        }
    }
}
