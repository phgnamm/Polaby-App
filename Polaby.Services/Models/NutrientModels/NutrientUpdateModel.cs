
using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.NutrientModels
{
    public class NutrientUpdateModel
    {
        public Guid? Id { get; set; } 

        [Range(0, float.MaxValue, ErrorMessage = "Post Processing Amount must be a non-negative value.")]
        public float? PostProcessingAmount { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Nutrition ID must be a non-negative value.")]
        public int? NutritionId { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Conversion Rate must be a non-negative value.")]
        public float? ConversionRate { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Amount must be a non-negative value.")]
        public float? Amount { get; set; }

        public string? UnitName { get; set; }

        public string? UnitCode { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Fractional Quantity must be a non-negative value.")]
        public int? FractionalQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Nutrition Code must be a non-negative value.")]
        public int? NutritionCode { get; set; }
    }
}
