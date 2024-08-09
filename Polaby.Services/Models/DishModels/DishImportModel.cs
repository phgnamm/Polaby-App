
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Polaby.Services.Models.DishModels
{
    public class DishImportModel
    {
        [Required(ErrorMessage = "Dish name is required!")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Dish image is required!")]
        public string? Image { get; set; }
        public List<Guid> IngredientIds { get; set; }
    }
}
