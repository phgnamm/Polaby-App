using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class IngredientSearchRepository : GenericRepository<IngredientSearch>, IIngredientSearchRepository
    {
        private readonly AppDbContext _dbContext;
        public IngredientSearchRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }
        public async Task<IngredientSearch> GetById(Guid id)
        {
            return await _dbContext.IngredientSearch
                .Include(cp => cp.IngredientSearchNutrients)
                .ThenInclude(cp => cp.Nutrient)
                .FirstOrDefaultAsync(cp => cp.Id == id);
        }
    }
}
