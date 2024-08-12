using Polaby.Repositories.Common;
using Polaby.Services.Common;

namespace Polaby.Services.Models.NotificationModels
{
    public class NotificationFilterModel : PaginationParameter
    {
        public string Order { get; set; } = "creation-date";
        public bool OrderByDescending { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
        public bool? IsRead { get; set; }
        public Guid? ReceiverId { get; set; }
        public Guid? SenderId { get; set; }
        public Guid? CommunityPostId { get; set; }
        public Guid? NotificationTypeId { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.DEFAULT_MAX_PAGE_SIZE;
    }
}
