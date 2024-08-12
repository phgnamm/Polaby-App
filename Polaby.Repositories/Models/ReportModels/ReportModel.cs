using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Models.ReportModels;

public class ReportModel : BaseEntity
{
    public string? Note { get; set; }
    public ReportReason Reason { get; set; }
    public ReportStatus Status { get; set; }
    public Guid? CommentId { get; set; }
    public Guid? CommunityPostId { get; set; }
}