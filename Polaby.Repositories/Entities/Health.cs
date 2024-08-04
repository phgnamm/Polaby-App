using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Entities
{
    public class Health : BaseEntity
    {
        public double Value { get; set; }
        public HealthUnit Unit { get; set; }
        public HealthType Type { get; set; }
        public DateOnly Date { get; set; }

        // Foreign key
        public Guid? UserId { get; set; }

        // Relationship
        public virtual Account? User { get; set; }
    }
}