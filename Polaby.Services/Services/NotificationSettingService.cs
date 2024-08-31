using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommunityPostModels;
using Polaby.Services.Models.NewFolder;
using Polaby.Services.Models.NotificationModels;
using Polaby.Services.Models.NotificationSettingModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services
{
    public class NotificationSettingService : INotificationSettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationSettingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDataModel<NotificationSettingModel>> Update(NotificationSettingUpdateModel notificationUpdateModel)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountById((Guid)notificationUpdateModel.AccountId);
            if (account == null)
            {
                return new ResponseDataModel<NotificationSettingModel>()
                {
                    Message = "Account not found",
                    Status = false
                };
            }

            var notificationType = await _unitOfWork.NotificationTypeRepository.GetAsync((Guid)notificationUpdateModel.NotificationTypeId);
            if (notificationType == null)
            {
                return new ResponseDataModel<NotificationSettingModel>()
                {
                    Message = "Notification type not found",
                    Status = false
                };
            }

            var existingNotificationSetting = await _unitOfWork.NotificationSettingRepository.GetByAccountAndType(notificationUpdateModel.AccountId, 
                notificationUpdateModel.NotificationTypeId);

            NotificationSetting notificationSetting = new();

            if (existingNotificationSetting == null)
            {
                notificationSetting = _mapper.Map<NotificationSetting>(notificationUpdateModel);  
                await _unitOfWork.NotificationSettingRepository.AddAsync(notificationSetting);
            }
            else
            {
                //_unitOfWork.DbContext.Entry(existingNotificationSetting).State = EntityState.Detached;
                notificationSetting = _mapper.Map(notificationUpdateModel, existingNotificationSetting);
                //notificationSetting = _mapper.Map<NotificationSetting>(notificationUpdateModel);
                _unitOfWork.NotificationSettingRepository.Update(notificationSetting);
            }
         
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<NotificationSettingModel>(notificationSetting);
            if (result != null)
            {
                return new ResponseDataModel<NotificationSettingModel>()
                {
                    Status = true,
                    Message = "Update notification setting successfully",
                    Data = result
                };
            }
            return new ResponseDataModel<NotificationSettingModel>()
            {
                Status = false,
                Message = "Update notification setting fail"
            };
        }

        public async Task<Pagination<NotificationSettingModel>> GetAllNotificationSettings(NotificationSettingFilterModel notificationSettingFilterModel)
        {
            var notificationSettingList = await _unitOfWork.NotificationSettingRepository.GetAllAsync(
            filter: x =>
                (notificationSettingFilterModel.AccountId == null || x.AccountId == notificationSettingFilterModel.AccountId),

            pageIndex: notificationSettingFilterModel.PageIndex,
            pageSize: notificationSettingFilterModel.PageSize,
            include: "NotificationType,Account"
        );

            if (notificationSettingList != null)
            {
                var notificationSettingDetailList = notificationSettingList.Data.Select(cp => new NotificationSettingModel
                {
                    Id = cp.Id,
                    IsEnabled = cp.IsEnabled,
                    AccountId = cp.Account.Id,
                    AccountName = cp.Account.FirstName + " " + cp.Account.LastName,
                    NotificationTypeId = cp.NotificationType.Id,
                    NotificationTypeName = cp.NotificationType.Name.ToString(),
                    NotificationTypeContent = cp.NotificationType.Content
                }).ToList();

                return new Pagination<NotificationSettingModel>(notificationSettingDetailList, notificationSettingList.TotalCount, notificationSettingFilterModel.PageIndex, notificationSettingFilterModel.PageSize);
            }
            return null;
        }
    }
}
