

using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class UserMenuRepository : GenericRepository<UserMenu>, IUserMenuRepository
    {
        private readonly AppDbContext _dbContext;

        public UserMenuRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }
        public async Task<List<UserMenu>> GetUserMenusAsync(Guid userId, Guid menuId)
        {
            return await _dbContext.UserMenu
                                   .Where(x => x.UserId == userId && x.MenuId == menuId)
                                   .ToListAsync();
        }
    }
}
