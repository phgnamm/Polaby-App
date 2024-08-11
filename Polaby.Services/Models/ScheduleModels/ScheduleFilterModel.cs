using Polaby.Repositories.Common;
using Polaby.Services.Common;

namespace Polaby.Services.Models.ScheduleModels
{
    public class ScheduleFilterModel : PaginationParameter
    {
        public string Order { get; set; } = "creation-date";
        public bool OrderByDescending { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
        public Guid? UserId { get; set; }
        public DateOnly? Date {  get; set; }
        //public string? Search { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.DEFAULT_MAX_PAGE_SIZE;
    }
}
