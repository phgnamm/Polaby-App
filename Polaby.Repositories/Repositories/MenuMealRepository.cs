using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class MenuMealRepository : GenericRepository<MenuMeal>, IMenuMealRepository
    {
        private readonly AppDbContext _dbContext;
        public MenuMealRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }
        public async Task<List<MenuMeal>> GetMenuMealsAsync(Guid menuId, Guid mealId)
        {
            return await _dbContext.MenuMeal
                                   .Where(x => x.MenuId == menuId && x.MealId == mealId)
                                   .ToListAsync();
        }
    }
}
