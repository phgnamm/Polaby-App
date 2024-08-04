using Polaby.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Entities
{
    public class Health: BaseEntity
    {
        public double Value { get; set; }
        public HealthUnit Unit { get; set; }
        public HealthType Type { get; set; }
        public DateTime Date { get; set; }

        // Foreign key
        public Guid? UserId { get; set; }

        // Relationship
        public virtual Account? User { get; set; }
    }
}
