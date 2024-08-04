namespace Polaby.Repositories.Entities
{
    public class UserMenu : BaseEntity
    {
        // Foreign key
        public Guid? UserId { get; set; }
        public Guid? MenuId { get; set; }

        // Relationship
        public virtual Account? User { get; set; }
        public virtual Menu? Menu { get; set; }
    }
}