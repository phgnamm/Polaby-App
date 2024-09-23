using AutoMapper;
using Polaby.API.Helper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommentLikeModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services
{
    public class CommentLikeService : ICommentLikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly OneSignalPushNotificationService _oneSignalPushNotificationService;

        public CommentLikeService(IUnitOfWork unitOfWork, IMapper mapper,
            OneSignalPushNotificationService oneSignalPushNotificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _oneSignalPushNotificationService = oneSignalPushNotificationService;

        }

        public async Task<ResponseModel> Like(CommentLikeModel commentLikeModel)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountById((Guid)commentLikeModel.UserId);
            if (account == null)
            {
                return new ResponseModel()
                {
                    Message = "User not found",
                    Status = false
                };
            }

            var comment = await _unitOfWork.CommentRepository.GetAsync((Guid)commentLikeModel.CommentId);
            if (comment == null)
            {
                return new ResponseModel()
                {
                    Message = "Comment not found",
                    Status = false
                };
            }

            CommentLike commentLike = _mapper.Map<CommentLike>(commentLikeModel);

            await _unitOfWork.CommentLikeRepository.AddAsync(commentLike);
            int check = await _unitOfWork.SaveChangeAsync();

            //CommentLikeResponseModel responseModel = _mapper.Map<CommentLike>(commentLikeModel);
            //CommentLikeResponseModel responseModel = new()
            //{
            //    CommentId = commentLike.CommentId,
            //    UserId = commentLike.Id,
            //    UserName = account.FirstName + " " + account.LastName,
            //};

            if(check != 0)
            {
                var notificationType = await _unitOfWork.NotificationTypeRepository.GetByName(NotificationTypeName.Like);
                var content = account.FirstName + " " + account.LastName + " " + notificationType.Content;

                _oneSignalPushNotificationService.SendNotificationAsync("Thích", content, commentLikeModel.SubscriptionId);
            }

            return new ResponseModel
            {
                Message = "Like successfully",
                Status = true
            };
        }

        public async Task<ResponseModel> Unlike(CommentLikeModel commentLikeModel)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountById((Guid)commentLikeModel.UserId);
            if (account == null)
            {
                return new ResponseModel()
                {
                    Message = "User not found",
                    Status = false
                };
            }

            var comment = await _unitOfWork.CommentRepository.GetAsync((Guid)commentLikeModel.CommentId);
            if (comment == null)
            {
                return new ResponseModel()
                {
                    Message = "Comment not found",
                    Status = false
                };
            }

            //CommentLike commentLike = _mapper.Map<CommentLike>(commentLikeModel);
            CommentLike commentLike = await _unitOfWork.CommentLikeRepository.GetByUserAndComment((Guid)commentLikeModel.UserId, (Guid)commentLikeModel.CommentId);

            if(commentLike != null)
            {
                _unitOfWork.CommentLikeRepository.HardDelete(commentLike);
                await _unitOfWork.SaveChangeAsync();
            }

            return new ResponseModel()
            {
                Message = "Unlike successfully",
                Status = true
            };
        }
    }
}
