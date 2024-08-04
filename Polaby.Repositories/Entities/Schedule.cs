namespace Polaby.Repositories.Entities
{
    public class Schedule : BaseEntity
    {
        public string? Title { get; set; }
        public string? Location { get; set; }
        public string? Note { get; set; }
        public DateOnly? Date { get; set; }

        // Foreign key
        public Guid? UserId { get; set; }

        // Relationship
        public virtual Account? User { get; set; }
    }
}