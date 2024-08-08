namespace Polaby.Repositories.Entities
{
    public class CommentLike : BaseEntity
    {
        // Foreign key
        public Guid? CommentId { get; set; }
        public Guid? UserId { get; set; }

        // Relationship
        public virtual Comment? Comment { get; set; }
        public virtual Account? User { get; set; }
    }
}
