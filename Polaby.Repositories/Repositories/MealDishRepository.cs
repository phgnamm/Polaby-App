
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class MealDishRepository : GenericRepository<MealDish>, IMealDishRepository
    {
        private readonly AppDbContext _dbContext;
        public MealDishRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }
    }
}
