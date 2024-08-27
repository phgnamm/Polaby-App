using Polaby.Repositories.Common;
using Polaby.Repositories.Enums;
using Polaby.Services.Common;
namespace Polaby.Services.Models.CommunityPostModels
{
    public class CommunityPostFilterModel : PaginationParameter
    {
        public string Order { get; set; } = "creation-date";
        public bool OrderByDescending { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
        public bool? IsProfessional { get; set; }
        public bool? IsFollowing { get; set; } = false; //filter bài viết chuyên gia đang follow
        public PostVisibility? Visibility { get; set; }
        public Guid? AccountId { get; set; }
        public string? Search { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.DEFAULT_MAX_PAGE_SIZE;
    }
}
