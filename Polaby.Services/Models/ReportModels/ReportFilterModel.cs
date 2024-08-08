using Polaby.Repositories.Enums;
using Polaby.Services.Common;

namespace Polaby.Services.Models.ReportModels;

public class ReportFilterModel : PaginationParameter
{
    public string Order { get; set; } = "";
    public bool OrderByDescending { get; set; } = true;
    public ReportReason? Reason { get; set; }
    public ReportStatus? Status { get; set; }
    public Guid? CommentId { get; set; }
    public Guid? CommunityPostId { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string? Search { get; set; }

    // protected override int MinPageSize { get; set; } = PaginationConstant.ACCOUNT_MIN_PAGE_SIZE;
    // protected override int MaxPageSize { get; set; } = PaginationConstant.ACCOUNT_MAX_PAGE_SIZE;
}