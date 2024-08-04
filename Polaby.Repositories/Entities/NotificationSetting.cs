
namespace Polaby.Repositories.Entities
{
    public class NotificationSetting : BaseEntity
    {
        public bool IsEnabled { get; set; }

        // Foreign key
        public Guid? UserId { get; set; }
        public Guid? TypeId { get; set; }

        // Relationship
        public virtual Account? User { get; set; }
        public virtual NotificationType? NotificationType { get; set; }

    }
}
