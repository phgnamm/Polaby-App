using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class IngredientSearchNutrientRepository : GenericRepository<IngredientSearchNutrient>, IIngredientSearchNutrientRepository
    {
        private readonly AppDbContext _dbContext;
        public IngredientSearchNutrientRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }
    }
}
