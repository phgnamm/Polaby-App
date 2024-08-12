using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.WeeklyPostModels;

public class WeeklyPostUpdateModel
{
    [Required(ErrorMessage = "Week is required")]
    [Range(1, 42, ErrorMessage = "Week must be from 1 to 42")]
    public int Week { get; set; } //Số tuần

    [Required(ErrorMessage = "About baby is required")]
    public string AboutBaby { get; set; }

    [Required(ErrorMessage = "About mother is required")]
    public string AboutMother { get; set; }

    [Required(ErrorMessage = "Advice is required")]
    public string Advice { get; set; }

    [Required(ErrorMessage = "Size is required")]
    [Range(0, 10, ErrorMessage = "Size must be from 0cm to 100cm")]
    public double Size { get; set; } //Kích thước thai

    [Required(ErrorMessage = "Weight is required")]
    [Range(0, 10, ErrorMessage = "Weight must be from 0g to 1000g")]
    public double Weight { get; set; } //Cân nặng thai

    public string? Source { get; set; }
    public string? SourceUrl { get; set; }
}