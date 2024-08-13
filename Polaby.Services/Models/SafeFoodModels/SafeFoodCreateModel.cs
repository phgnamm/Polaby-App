using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.SafeFoodModels;
public class SafeFoodCreateModel
{
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }
    public string? Description { get; set; }

    [Required(ErrorMessage = "IsSafe status is required")]
    public bool IsSafe { get; set; }
    public string? Source { get; set; }
    public string? SourceUrl { get; set; }
}
