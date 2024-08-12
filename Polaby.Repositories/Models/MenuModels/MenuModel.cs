using Polaby.Repositories.Models.NutrientModels;

namespace Polaby.Repositories.Models.MenuModels
{
    public class MenuModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public float? Kcal { get; set; }
        public float? Protein { get; set; }
        public float? Carbohydrates { get; set; }
        public float? Fat { get; set; }
        public float? Alco { get; set; }
        public float? Fiber { get; set; }
        public List<NutrientModel>? Nutrients { get; set; }
    }
}
