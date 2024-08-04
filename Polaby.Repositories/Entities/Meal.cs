using Polaby.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Entities
{
    public class Meal : BaseEntity
    {
        public MealName? Name { get; set; }
        public float? Kcal { get; set; }

        // Relationship
        public virtual ICollection<MenuMeal> MenuMeals { get; set; } = new List<MenuMeal>();
        public virtual ICollection<MealDish> MealDishes { get; set; } = new List<MealDish>();
    }
}
