

using Microsoft.EntityFrameworkCore;
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

        public async Task<List<DishIngredient>> GetDishIngredientsAsync(Guid dishId, Guid ingredientId)
        {
            return await _dbContext.DishIngredient
                                   .Where(x => x.DishId == dishId && x.IngredientId == ingredientId)
                                   .ToListAsync();
        }
    }
}
