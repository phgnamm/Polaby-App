using Polaby.Repositories.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.HealthModels
{
    public class HealthUpdateModel
    {
        [Required(ErrorMessage = "Health Type is required.")]
        public HealthType Type { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Value must be a non-negative number.")]
        public double Value { get; set; }

        [Required(ErrorMessage = "Health Unit is required.")]
        public HealthUnit Unit { get; set; }
    }
}
