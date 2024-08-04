namespace Polaby.Repositories.Entities
{
    public class Rating : BaseEntity
    {
        public int Star { get; set; }
        public string? Comment { get; set; }

        // Foreign key
        public Guid? ExpertId { get; set; }
        public Guid? UserId { get; set; }

        // Relationship
        public virtual Account? Expert { get; set; }
        public virtual Account? User { get; set; }
    }
}
