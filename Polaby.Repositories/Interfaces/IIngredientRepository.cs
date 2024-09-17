using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces
{
    public interface IIngredientRepository : IGenericRepository<Ingredient>
    {
        Task<Ingredient> GetById(Guid id);
    }
}
