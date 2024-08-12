using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces
{
    public interface IUserMenuRepository : IGenericRepository<UserMenu>
    {
        Task<List<UserMenu>> GetUserMenusAsync(Guid userId, Guid menuId);
    }
}
