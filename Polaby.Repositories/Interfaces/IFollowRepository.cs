using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces
{
    public interface IFollowRepository : IGenericRepository<Follow>
    {
        Task<Follow> GetByUserAndExpert(Guid userId, Guid expertId);
    }
}
