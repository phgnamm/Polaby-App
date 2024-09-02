
using Polaby.Repositories.Entities;
using Polaby.Repositories.Models.IngredientModels;
using Polaby.Repositories.Models.NutrientModels;

namespace Polaby.Repositories.Models.DishModels
{
    public class DishModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public float? Kcal { get; set; }
        public float? Weight { get; set; }
        public float? Protein { get; set; }
        public float? Carbohydrates { get; set; }
        public float? Fat { get; set; }
        public List<IngredientModel>? Ingredients { get; set; }
        public List<NutrientModel>? Nutrients { get; set; }
    }
}
