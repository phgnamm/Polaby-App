

using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces
{
    public interface IIngredientSearchRepository : IGenericRepository<IngredientSearch>
    {
        Task<IngredientSearch> GetById(Guid id);
    }
}
