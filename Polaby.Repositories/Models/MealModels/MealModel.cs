
namespace Polaby.Repositories.Models.MenuModels
{
    public class MealModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public float? Kcal { get; set; }
        public List<MealDishModel> MealDishes { get; set; } = new List<MealDishModel>();
    }
}
