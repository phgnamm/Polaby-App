
namespace Polaby.Repositories.Models.IngredientModels
{
    public class IngredientModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public float? Kcal { get; set; }
        public float? Water { get; set; }
        public float? Protein { get; set; }
        public float? Carbohydrates { get; set; } // Tổng lượng carbohydrate
        public float? Starch { get; set; } // Lượng tinh bột     
        public float? Fat { get; set; }
        public float? Fiber { get; set; }
        public float? Sugar { get; set; }
        public float? SaturatedFat { get; set; }
        public float? MonounsaturatedFat { get; set; }
        public float? PolyunsaturatedFat { get; set; }
        public float? Cholesterol { get; set; }
        public float? Sodium { get; set; }
        public float? Potassium { get; set; }
        public float? Calcium { get; set; }
        public float? Iron { get; set; }
        public float? Magnesium { get; set; }
        public float? Zinc { get; set; }
        public string? Source { get; set; }
        public string? SourceUrl { get; set; }
    }
}
