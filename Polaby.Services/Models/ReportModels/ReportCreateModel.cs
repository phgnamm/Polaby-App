using System.ComponentModel.DataAnnotations;
using Polaby.Repositories.Enums;

namespace Polaby.Services.Models.ReportModels;

public class ReportCreateModel
{
    public string? Note { get; set; }
    [Required(ErrorMessage = "Reason is required")]
    public ReportReason Reason { get; set; }
    public Guid? CommentId { get; set; }
    public Guid? CommunityPostId { get; set; }
}