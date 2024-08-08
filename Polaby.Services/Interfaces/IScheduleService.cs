using Polaby.Services.Models.ResponseModels;
using Polaby.Services.Models.ScheduleModels;

namespace Polaby.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<ResponseDataModel<ScheduleModel>> Create(ScheduleCreateModel scheduleCreateModel);
        Task<ResponseDataModel<ScheduleModel>> Update(Guid id, ScheduleUpdateModel scheduleUpdateModel);
        Task<ResponseModel> Delete(Guid id);
    }
}
