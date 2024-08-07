using System;
using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.MenuModels
{
    public class MenuImportModel
    {
        [Required(ErrorMessage = "Menu name is required!")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Description is required!")]
        public string? Description { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "Water is required!")]
        [Range(0.01, 5000, ErrorMessage = "Water must be greater than 0 and less than or equal to 5000.")]
        public float? Water { get; set; }
        public List<Guid> MealIds { get; set; }

    }
}
