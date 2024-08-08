using System.ComponentModel.DataAnnotations;
using Polaby.Repositories.Enums;

namespace Polaby.Services.Models.ReportModels;

public class ReportUpdateModel
{
    [Required(ErrorMessage = "Status is required")]
    public ReportStatus Status { get; set; }
}