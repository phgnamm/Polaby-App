using Polaby.Services.Common;
using Polaby.Services.Models.NotificationModels;
using Polaby.Services.Models.NotificationSettingModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces
{
    public interface INotificationSettingService
    {
        Task<ResponseDataModel<NotificationSettingModel>> Update(NotificationSettingUpdateModel notificationUpdateModel);
        Task<Pagination<NotificationSettingModel>> GetAllNotificationSettings(NotificationSettingFilterModel notificationSettingFilterModel);
    }
}
