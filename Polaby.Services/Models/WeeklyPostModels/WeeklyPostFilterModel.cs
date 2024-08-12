using Polaby.Services.Common;

namespace Polaby.Services.Models.WeeklyPostModels;

public class WeeklyPostFilterModel : PaginationParameter
{
    public string Order { get; set; } = "";
    public bool OrderByDescending { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public string? Search { get; set; }
}