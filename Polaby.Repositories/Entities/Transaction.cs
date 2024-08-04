using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Entities
{
    public class Transaction : BaseEntity
    {
        public string? Code { get; set; }
        public string? Message { get; set; }
        public TransactionType TransactionType { get; set; }
        public float Amount { get; set; }
        public TransactionStatus Status { get; set; }

        // Foreign key
        public Guid? UserId { get; set; }

        // Relationship
        public virtual Account? User { get; set; }
    }
}