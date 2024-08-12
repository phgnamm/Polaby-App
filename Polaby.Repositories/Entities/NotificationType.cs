using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Entities
{
    public class NotificationType : BaseEntity
    {
        public NotificationTypeName? Name { get; set; }
        public string? Content { get; set; }

        // Relationship
        public virtual ICollection<NotificationSetting> NotificationSettings { get; set; } =
            new List<NotificationSetting>();
    }
}