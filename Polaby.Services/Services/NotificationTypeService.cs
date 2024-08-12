using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommentModels;
using Polaby.Services.Models.NotificationTypeModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services
{
    public class NotificationTypeService : INotificationTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDataModel<NotificationTypeModel>> Update(NotificationTypeModel notificationModel)
        {
            var existingNotificationType = await _unitOfWork.NotificationTypeRepository.GetAsync(notificationModel.Id);
            if (existingNotificationType == null)
            {
                return new ResponseDataModel<NotificationTypeModel>()
                {
                    Message = "Type of notification not found",
                    Status = false
                };
            }

            existingNotificationType = _mapper.Map(notificationModel, existingNotificationType);
            _unitOfWork.NotificationTypeRepository.Update(existingNotificationType);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<NotificationTypeModel>(existingNotificationType);
            if (result != null)
            {
                return new ResponseDataModel<NotificationTypeModel>()
                {
                    Status = true,
                    Message = "Update Type of notification successfully",
                    Data = result
                };
            }
            return new ResponseDataModel<NotificationTypeModel>()
            {
                Status = false,
                Message = "Update Type of notification fail"
            };
        }
    }
}
