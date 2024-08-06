
namespace Polaby.Repositories.Models.MenuModels
{
    public class MenuMealModel
    {
        public Guid MealId { get; set; }
        public MealModel Meal { get; set; } = new MealModel();
    }
}
