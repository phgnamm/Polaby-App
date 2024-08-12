using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces
{
    public interface INotificationSettingRepository : IGenericRepository<NotificationSetting>
    {
        Task<NotificationSetting> GetByAccountAndType(Guid? accountId, Guid? notificationTypeId);
    }
}
