

using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class DishIngredientRepository : GenericRepository<DishIngredient>, IDishIngredientRepository
    {
        private readonly AppDbContext _dbContext;
        public DishIngredientRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }
    }
}
