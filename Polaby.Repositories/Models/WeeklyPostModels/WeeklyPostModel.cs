using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Models.WeeklyPostModels;

public class WeeklyPostModel : BaseEntity
{
    public int Week { get; set; } //Số tuần
    public string AboutBaby { get; set; }
    public string AboutMother { get; set; }
    public string Advice { get; set; }
    public double Size { get; set; } //Kích thước thai
    public double Weight { get; set; } //Cân nặng thai
    public string? Source { get; set; }
    public string? SourceUrl { get; set; }
}