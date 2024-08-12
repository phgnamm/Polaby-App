namespace Polaby.Repositories.Entities
{
    public class Menu : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public float? Kcal { get; set; }
        public float? Protein { get; set; }
        public float? Carbohydrates { get; set; }
        public float? Fat { get; set; }
        public float? Alco { get; set; }
        public float? Fiber { get; set; }

        // Relationship
        public virtual ICollection<UserMenu> UserMenus { get; set; } = new List<UserMenu>();
        public virtual ICollection<MenuMeal> MenuMeals { get; set; } = new List<MenuMeal>();
        public virtual ICollection<Nutrient> Nutrients { get; set; } = new List<Nutrient>();
    }
}