using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces
{
    public interface IMenuMealRepository : IGenericRepository<MenuMeal>
    {
        Task<List<MenuMeal>> GetMenuMealsAsync(Guid menuId, Guid mealId);
    }
}
