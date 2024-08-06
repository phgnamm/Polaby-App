
namespace Polaby.Repositories.Models.MenuModels
{
    public class MealDishModel
    {
        public Guid DishId { get; set; }
        public DishModel Dish { get; set; } = new DishModel();
    }
}
