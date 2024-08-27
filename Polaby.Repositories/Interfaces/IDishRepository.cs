using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces
{
    public interface IDishRepository : IGenericRepository<Dish>
    {
        Task<Dish> GetById(Guid id);
    }
}
