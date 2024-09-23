using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Models.CommentLikeModels
{
    public class CommentLikeResponseModel
    {
        public Guid? CommentId { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public string? ContentNotification { get; set; }
    }
}
