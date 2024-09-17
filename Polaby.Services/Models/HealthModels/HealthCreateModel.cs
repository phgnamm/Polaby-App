using Polaby.Repositories.Enums;
using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.HealthModels
{
    public class HealthCreateModel
    {
        [Required(ErrorMessage = "UserId is required.")]
        public Guid? UserId { get; set; }

        [Required(ErrorMessage = "Health Type is required.")]
        public HealthType Type { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Value must be a non-negative number.")]
        public double Value { get; set; }

        [Required(ErrorMessage = "Health Unit is required.")]
        public HealthUnit Unit { get; set; }
        public DateOnly? Date { get; set; }
    }
}
