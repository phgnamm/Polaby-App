using Polaby.Repositories.Enums;
using Polaby.Services.Models.NutrientModels;
using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.IngredientModels
{
    public class IngredientImportModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Url(ErrorMessage = "Invalid image URL format.")]
        public string? Image { get; set; }

        public bool Animal { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Kcal must be a non-negative value.")]
        public float? Kcal { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "KcalDefault must be a non-negative value.")]
        public float? KcalDefault { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Weight must be a non-negative value.")]
        public float? Weight { get; set; }

        public Unit? Unit { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "NumberOfDecimalPart must be a non-negative value.")]
        public float? NumberOfDecimalPart { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "DisposalRate must be a non-negative value.")]
        public float? DisposalRate { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "FoodGroupId must be a non-negative value.")]
        public int? FoodGroupId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "IndexFoodGroup must be a non-negative value.")]
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

        [Url(ErrorMessage = "Invalid source URL format.")]
        public string? SourceUrl { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Index must be a non-negative value.")]
        public int? Index { get; set; }
        public List<NutrientImportModel>? Nutrients { get; set; }
    }
}
