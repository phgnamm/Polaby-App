using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Entities
{
    public class MealDish : BaseEntity
    {
        // Foreign key
        public Guid? MealId { get; set; }
        public Guid? DishId { get; set; }

        // Relationship
        public virtual Meal? Meal { get; set; }
        public virtual Dish? Dish { get; set; }
    }
}
