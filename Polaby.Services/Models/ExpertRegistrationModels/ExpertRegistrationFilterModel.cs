using Polaby.Repositories.Enums;
using Polaby.Services.Common;

namespace Polaby.Services.Models.ExpertRegistrationModels;

public class ExpertRegistrationFilterModel : PaginationParameter
{
    public string Order { get; set; } = "";
    public bool OrderByDescending { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public string? Search { get; set; }
    public int? MinYearsOfExperience { get; set; }
    public int? MaxYearsOfExperience { get; set; }
    public Level? Level { get; set; }
    public ExpertRegistrationStatus? Status { get; set; }
}