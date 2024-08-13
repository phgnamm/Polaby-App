using Polaby.Repositories.Common;
using Polaby.Services.Common;

namespace Polaby.Services.Models.HealthModels
{
    public class HealthFilterModel : PaginationParameter
    {
        public string Order { get; set; } = "";
        public bool OrderByDescending { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid? UserId { get; set; }
        public DateOnly Date { get; set; }
        public bool FilterWeight { get; set; } = false;
        public bool FilterHeight { get; set; } = false;
        public bool FilterSize { get; set; } = false;
        public bool FilterBloodPressureSys { get; set; } = false;
        public bool FilterBloodPressureDia { get; set; } = false;
        public bool FilterHeartbeat { get; set; } = false;
        public bool FilterContractility { get; set; } = false;
        public string? Search { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.DEFAULT_MAX_PAGE_SIZE;
    }
}
