using Polaby.Repositories.Models.NutrientModels;

namespace Polaby.Repositories.Models.IngredientModels
{
    public class IngredientModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public float? Kcal { get; set; }
        public float? Weight { get; set; } // Weight of the ingredient
        public float? NumberOfDecimalPart { get; set; } // Number of decimal parts for the ingredient
        public float? DisposalRate { get; set; } // Disposal rate
        public int? FoodGroupId { get; set; } // Food group ID
        public int? IndexFoodGroup { get; set; } // Index for the food group
        public string? FoodGroup { get; set; } // Name or description of the food group
        public float? Protein { get; set; }
        public float? Carbohydrates { get; set; }
        public float? Fat { get; set; }
        public float? Alco { get; set; } // Amount of alcohol
        public string? Source { get; set; }
        public string? SourceUrl { get; set; }
        public int? Index { get; set; } // Index for the ingredient
        public List<NutrientModel>? Nutrients { get; set; }
    }
}
