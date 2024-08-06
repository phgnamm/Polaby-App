
namespace Polaby.Repositories.Models.MenuModels
{
    public class DishIngredientModel
    {
        public Guid IngredientId { get; set; }
        public IngredientModel Ingredient { get; set; } = new IngredientModel();
    }
}
