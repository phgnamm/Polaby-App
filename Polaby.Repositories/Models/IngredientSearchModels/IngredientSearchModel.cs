

using Polaby.Repositories.Models.NutrientModels;

namespace Polaby.Repositories.Models.IngredientSearchModels
{
    public class IngredientSearchModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public float? Kcal { get; set; }
        public float? DisposalRate { get; set; } // Disposal rate
        public int? FoodGroupId { get; set; } // Food group ID
        public int? IndexFoodGroup { get; set; } // Index for the food group
        public string? FoodGroup { get; set; } // Name or description of the food group
        public float? Protein { get; set; }
        public float? Carbohydrates { get; set; }
        public float? Fat { get; set; }
        public float? Water { get; set; } // Amount of alcohol
        public string? Source { get; set; }
        public string? SourceUrl { get; set; }
        public List<NutrientModel>? Nutrients { get; set; }
    }
}
