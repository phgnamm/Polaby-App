namespace Polaby.Repositories.Entities
{
    public class Notification : BaseEntity
    {
        public bool IsRead { get; set; }

        // Foreign key
        public Guid? ReceiverId { get; set; }
        public Guid? SenderId { get; set; }
        public Guid? CommunityPostId { get; set; }
        public Guid? NotificationTypeId { get; set; }

        // Relationship
        public virtual Account? Receiver { get; set; }
        public virtual Account? Sender { get; set; }
        public virtual CommunityPost? Post { get; set; }
        public virtual NotificationType? NotificationType { get; set; }
    }
}