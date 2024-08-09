using Polaby.Repositories.Enums;
using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.MealModels
{
    public class MealImportModel
    {
        [Required(ErrorMessage = "Meal name is required!")]
        public MealName? Name { get; set; }
        [Required(ErrorMessage = "Kcal is required!")]
        public List<Guid> DishIds { get; set; }

    }
}
