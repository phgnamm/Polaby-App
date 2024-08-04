using Polaby.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Entities
{
    public class Notification : BaseEntity
    {
        public bool IsRead { get; set; }
        
        // Foreign key
        public Guid? ReceiverId { get; set; }
        public Guid? SenderId { get; set; }
        public Guid? CommunityPostId { get; set; }
        public Guid? TypeId { get; set; }

        // Relationship
        public virtual Account? Reciever { get; set; }
        public virtual Account? Sender { get; set; }
        public virtual CommunityPost? Post { get; set; }
        public virtual NotificationType? NotificationType { get; set; }
    }
}
