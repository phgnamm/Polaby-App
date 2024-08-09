
using Polaby.Repositories.Enums;
using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.MealModels
{
    public class MealUpdateModel
    {
        [Required(ErrorMessage = "Name is required!")]
        public MealName Name { get; set; }
        public List<Guid> DishIds { get; set; }
    }
}
