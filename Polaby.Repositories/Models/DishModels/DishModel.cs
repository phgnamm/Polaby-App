﻿
namespace Polaby.Repositories.Models.MenuModels
{
    public class DishModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public float? Kcal { get; set; }
        public float? Protein { get; set; }
        public float? Starch { get; set; }
        public float? Fat { get; set; }
        public List<DishIngredientModel> DishIngredients { get; set; } = new List<DishIngredientModel>();
    }
}
