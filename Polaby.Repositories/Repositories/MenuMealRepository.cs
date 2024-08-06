
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
    }
}
