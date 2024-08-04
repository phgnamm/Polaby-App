namespace Polaby.Repositories.Entities
{
    public class DishIngredient : BaseEntity
    {
        // Foreign key
        public Guid? DishId { get; set; }
        public Guid? IngredientId { get; set; }

        // Relationship
        public virtual Dish? Dish { get; set; }
        public virtual Ingredient? Ingredient { get; set; }
    }
}