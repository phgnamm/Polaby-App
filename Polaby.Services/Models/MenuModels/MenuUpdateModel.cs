using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.MenuModels
{
    public class MenuUpdateModel
    {
        [Required(ErrorMessage = "Name is required!")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Image is required!")]
        public string? Image { get; set; }

        [Range(1, 5000, ErrorMessage = "Water must be greater than 0 and less than 5000")]
        public float? Water { get; set; }

        public List<Guid> MealIds { get; set; } = new List<Guid>();
    }
}
