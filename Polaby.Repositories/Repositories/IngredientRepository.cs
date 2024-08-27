using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class IngredientRepository : GenericRepository<Ingredient>, IIngredientRepository
    {
        private readonly AppDbContext _dbContext;
        public IngredientRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<Ingredient> GetById(Guid id)
        {
            return await _dbContext.Ingredient
                .Include(cp => cp.DishIngredients)
                .Include(cp => cp.Nutrients)
                .FirstOrDefaultAsync(cp => cp.Id == id);
        }
    }
}
