using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Entities
{
    public class CommunityPostLike : BaseEntity
    {
        // Foreign key
        public Guid? CommunityPostId { get; set; }
        public Guid? UserId { get; set; }

        // Relationship
        public virtual CommunityPost? CommunityPost { get; set; }
        public virtual Account? User { get; set; }
    }
}
