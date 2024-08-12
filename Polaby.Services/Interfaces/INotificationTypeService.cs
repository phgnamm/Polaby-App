using Polaby.Services.Models.NotificationTypeModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces
{
    public interface INotificationTypeService
    {
        Task<ResponseDataModel<NotificationTypeModel>> Update(NotificationTypeModel notificationModel);
    }
}
