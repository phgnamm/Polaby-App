using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces
{
    public interface IMealDishRepository : IGenericRepository<MealDish>
    {
        Task<List<MealDish>> GetMealDishesAsync(Guid mealId, Guid dishId);
    }
}
