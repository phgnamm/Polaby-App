using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Entities
{
    public class Note : BaseEntity
    {
        public string? Title { get; set; }
        public Trimester Trimester { get; set; }
        public DateTime Date { get; set; }

        // Foreign key
        public Guid? UserId { get; set; }

        // Relationship
        public virtual Account? User { get; set; }
    }
}
