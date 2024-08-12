using Polaby.Services.Common;
using Polaby.Services.Models.NewFolder;
using Polaby.Services.Models.NotificationModels;

namespace Polaby.Services.Interfaces
{
    public interface INotificationService
    {
        Task<Pagination<NotificationModel>> GetAllNotifications(NotificationFilterModel notificationFilterModel);
    }
}
