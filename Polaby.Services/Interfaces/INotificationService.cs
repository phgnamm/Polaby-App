using Polaby.Services.Common;
using Polaby.Services.Models.NewFolder;
using Polaby.Services.Models.NotificationModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces
{
    public interface INotificationService
    {
        Task<Pagination<NotificationModel>> GetAllNotifications(NotificationFilterModel notificationFilterModel);
        Task<ResponseDataModel<NotificationModel>> GetById(Guid id);
    }
}
