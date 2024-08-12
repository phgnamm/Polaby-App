
using Polaby.Services.Models.NutrientModels;
using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.DishModels
{
    public class DishUpdateModel
    {
        [Required(ErrorMessage = "Dish name is required!")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Dish image is required!")]
        public string? Image { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Kcal must be a non-negative value.")]
        public float? Kcal { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Weight must be a non-negative value.")]
        public float? Weight { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Protein must be a non-negative value.")]
        public float? Protein { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Carbohydrates must be a non-negative value.")]
        public float? Carbohydrates { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Fat must be a non-negative value.")]
        public float? Fat { get; set; }
        public List<NutrientUpdateModel>? Nutrients { get; set; }
    }
}
