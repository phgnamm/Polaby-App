using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Entities
{
    public class Report : BaseEntity
    {
        public string? Note { get; set; }
        public ReportReason? Reason { get; set; }
        public ReportStatus? Status { get; set; }

        // Foreign key
        public Guid? CommentId { get; set; }
        public Guid? CommunityPostId { get; set; }

        // Relationship
        public virtual Comment? Comment { get; set; }
        public virtual CommunityPost? CommunityPost { get; set; }
    }
}