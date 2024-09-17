using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class DishRepository : GenericRepository<Dish>, IDishRepository
    {
        private readonly AppDbContext _dbContext;
        public DishRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<Dish> GetById(Guid id)
        {
            return await _dbContext.Dish
                .Include(cp => cp.DishIngredients)
                .Include(cp => cp.Nutrients)
                .FirstOrDefaultAsync(cp => cp.Id == id);
        }
    }
}
