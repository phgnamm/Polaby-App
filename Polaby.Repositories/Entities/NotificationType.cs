using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Entities
{
    public class NotificationType : BaseEntity
    {
        public string? Name { get; set; }
        public string? Content { get; set; }

        // Relationship
        public virtual Notification? Notification { get; set; }
        public virtual ICollection<NotificationSetting> NotificationSettings { get; set; } = new List<NotificationSetting>();
    }
}
