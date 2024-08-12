using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces
{
    public interface IDishIngredientRepository : IGenericRepository<DishIngredient>
    {
        Task<List<DishIngredient>> GetDishIngredientsAsync(Guid dishId, Guid ingredientId);
    }
}
