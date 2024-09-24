using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Interfaces
{
    public interface INotificationTypeRepository : IGenericRepository<NotificationType>
    {
        Task<NotificationType> GetByName(NotificationTypeName name);
    }
}
