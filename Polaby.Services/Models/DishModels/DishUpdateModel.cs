
using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.DishModels
{
    public class DishUpdateModel
    {
        [Required(ErrorMessage = "Dish name is required!")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Dish image is required!")]
        public string? Image { get; set; }
        public List<Guid> IngredientIds { get; set; }
    }
}
