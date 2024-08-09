using Polaby.Repositories.Enums;
using Polaby.Services.Models.NutrientModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.IngredientModels
{
    public class IngredientUpdateModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Kcal must be a non-negative value.")]
        public float? Kcal { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "KcalDefault must be a non-negative value.")]
        public float? KcalDefault { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Weight must be a non-negative value.")]
        public float? Weight { get; set; }

        public Unit? Unit { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Number of Decimal Part must be a non-negative value.")]
        public float? NumberOfDecimalPart { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Disposal Rate must be a non-negative value.")]
        public float? DisposalRate { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Food Group ID must be a non-negative value.")]
        public int? FoodGroupId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Index Food Group must be a non-negative value.")]
        public int? IndexFoodGroup { get; set; }

        public string? FoodGroup { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Protein must be a non-negative value.")]
        public float? Protein { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Carbohydrates must be a non-negative value.")]
        public float? Carbohydrates { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Fat must be a non-negative value.")]
        public float? Fat { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Alco must be a non-negative value.")]
        public float? Alco { get; set; }

        public string? Source { get; set; }
        public string? SourceUrl { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Index must be a non-negative value.")]
        public int? Index { get; set; }
        public List<NutrientUpdateModel>? Nutrients { get; set; }
    }
}
