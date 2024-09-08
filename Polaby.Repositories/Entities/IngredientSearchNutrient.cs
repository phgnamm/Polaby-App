
namespace Polaby.Repositories.Entities
{
    public class IngredientSearchNutrient : BaseEntity
    {
        public Guid? IngredientSearchId { get; set; }
        public Guid? NutrientId { get; set; }

        // Relationship
        public virtual IngredientSearch? IngredientSearch { get; set; }
        public virtual Nutrient? Nutrient { get; set; }
    }
}
