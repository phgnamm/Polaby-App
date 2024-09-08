using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Models.IngredientSearchModels
{
    public class IngredientSearchUpdateModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        public string? Image { get; set; }

        public bool Animal { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Kcal must be a non-negative value.")]
        public float? Kcal { get; set; }

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

        [Range(0, float.MaxValue, ErrorMessage = "Water must be a non-negative value.")]
        public float? Water { get; set; }

        public string? Source { get; set; }

        public string? SourceUrl { get; set; }
    }
}
