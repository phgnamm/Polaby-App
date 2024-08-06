
using Polaby.Repositories.Common;
using Polaby.Services.Common;

namespace Polaby.Services.Models.MenuModels
{
    public class MenuFilterModel : PaginationParameter
    {
        public string Order { get; set; } = "";
        public bool OrderByDescending { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public List<int>? KcalValues { get; set; } = new List<int>
    {
        1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800, 1900, 2000, 2100,
        2200, 2300, 2400, 2500, 2600, 2700, 2800, 2900, 3000, 3100, 3200, 3300,
        3400, 3500, 3600, 3700, 3800, 3900, 4000
    };
        public string? Search { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.DEFAULT_MAX_PAGE_SIZE;
    }
}
