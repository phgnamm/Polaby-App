using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Entities
{
    public class Schedule : BaseEntity
    {
        public string? Title { get; set; }
        public string? Location { get; set; }
        public string? Note { get; set; }
        public DateTime Date { get; set; }

        // Foreign key
        public Guid? UserId { get; set; }

        // Relationship
        public virtual Account? User { get; set; }
    }
}
