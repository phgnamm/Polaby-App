namespace Polaby.Repositories.Entities
{
    public class Menu : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public float? Kcal { get; set; }
        public float? Water { get; set; }

        // Relationship
        public virtual ICollection<UserMenu> UserMenus { get; set; } = new List<UserMenu>();
        public virtual ICollection<MenuMeal> MenuMeals { get; set; } = new List<MenuMeal>();
    }
}