
namespace Polaby.Services.Models.DishModels
{
    public class DishIngredientCreateModel
    {
        public Guid DishId { get; set; }
        public List<Guid> IngredientIds { get; set; }
    }
}
