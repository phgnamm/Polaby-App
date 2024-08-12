
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<MealDish>> GetMealDishesAsync(Guid mealId, Guid dishId)
        {
            return await _dbContext.MealDish
                                   .Where(x => x.MealId == mealId && x.DishId == dishId)
                                   .ToListAsync();
        }
    }
}
