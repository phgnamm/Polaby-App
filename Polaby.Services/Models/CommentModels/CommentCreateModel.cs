using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Models.CommentModels
{
    public class CommentCreateModel
    {
        public string Content { get; set; }
        public string? Attachments { get; set; }
        public Guid AccountId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string? SubscriptionId { get; set; }
    }
}
