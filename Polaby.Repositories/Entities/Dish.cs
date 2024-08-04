using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Entities
{
    public class Dish : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public float? Kcal { get; set; }
        public float? Protein { get; set; }
        public float? Starch { get; set; }
        public float? Fat { get; set; }

        // Relationship
        public virtual ICollection<MealDish> MealDishes { get; set; } = new List<MealDish>();
        public virtual ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
    }
}
