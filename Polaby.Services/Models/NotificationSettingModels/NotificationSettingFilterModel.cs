using Polaby.Repositories.Common;
using Polaby.Services.Common;

namespace Polaby.Services.Models.NotificationSettingModels
{
    public class NotificationSettingFilterModel : PaginationParameter
    {
        public Guid? AccountId { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.DEFAULT_MAX_PAGE_SIZE;
    }
}
