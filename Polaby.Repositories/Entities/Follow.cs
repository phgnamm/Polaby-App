namespace Polaby.Repositories.Entities
{
    public class Follow : BaseEntity
    {
        // Foreign key
        public Guid? ExpertId { get; set; }
        public Guid? UserId { get; set; }

        // Relationship
        public virtual Account? Expert { get; set; }
        public virtual Account? User { get; set; }
    }
}