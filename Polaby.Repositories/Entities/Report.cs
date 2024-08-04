using Polaby.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
