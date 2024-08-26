using System.ComponentModel.DataAnnotations;
using Polaby.Repositories.Enums;

namespace Polaby.Services.Models.ExpertRegistrationModels;

public class ExpertRegistrationUpdateStatusModel
{
    [Required(ErrorMessage = "Status is required")]
    public ExpertRegistrationStatus Status { get; set; }

    public string? Note { get; set; }
}