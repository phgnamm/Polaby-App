using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class MenuRepository : GenericRepository<Menu>, IMenuRepository
    {
        private readonly AppDbContext _dbContext;
        public MenuRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<Menu> GetById(Guid id)
        {
            return await _dbContext.Menu
                .Include(cp => cp.Nutrients)
                .FirstOrDefaultAsync(cp => cp.Id == id);
        }
    }
}
