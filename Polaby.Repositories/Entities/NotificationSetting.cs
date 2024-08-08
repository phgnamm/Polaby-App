namespace Polaby.Repositories.Entities
{
    public class NotificationSetting : BaseEntity
    {
        public bool IsEnabled { get; set; }

        // Foreign key
        public Guid? AccountId { get; set; }
        public Guid? TypeId { get; set; }

        // Relationship
        public virtual Account? Account { get; set; }
        public virtual NotificationType? NotificationType { get; set; }
    }
}