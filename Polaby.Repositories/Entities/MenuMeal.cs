namespace Polaby.Repositories.Entities
{
    public class MenuMeal : BaseEntity
    {
        // Foreign key
        public Guid? MenuId { get; set; }
        public Guid? MealId { get; set; }

        // Relationship
        public virtual Menu? Menu { get; set; }
        public virtual Meal? Meal { get; set; }
    }
}