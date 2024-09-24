using AutoMapper;
using Polaby.API.Helper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommentLikeModels;
using Polaby.Services.Models.CommunityPostLikeModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services
{
    public class CommunityPostLikeService : ICommunityPostLikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly OneSignalPushNotificationService _oneSignalPushNotificationService;

        public CommunityPostLikeService(IUnitOfWork unitOfWork, IMapper mapper,
            OneSignalPushNotificationService oneSignalPushNotificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _oneSignalPushNotificationService = oneSignalPushNotificationService;
        }

        public async Task<ResponseModel> Like(CommunityPostLikeModel communityPostLikeModel)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountById((Guid)communityPostLikeModel.UserId);
            if (account == null)
            {
                return new ResponseModel()
                {
                    Message = "User not found",
                    Status = false
                };
            }

            var post = await _unitOfWork.CommunityPostRepository.GetAsync((Guid)communityPostLikeModel.CommunityPostId);
            if (post == null)
            {
                return new ResponseModel()
                {
                    Message = "Post not found",
                    Status = false
                };
            }

            CommunityPostLike communityPostLike = _mapper.Map<CommunityPostLike>(communityPostLikeModel);

            await _unitOfWork.CommunityPostLikeRepository.AddAsync(communityPostLike);
            int check = await _unitOfWork.SaveChangeAsync();

            if(check != 0)
            {
                var notificationType = await _unitOfWork.NotificationTypeRepository.GetByName(NotificationTypeName.Like);
                var content = account.FirstName + " " + account.LastName + " " + notificationType.Content;
                _oneSignalPushNotificationService.SendNotificationAsync("Thích", content, communityPostLikeModel.SubscriptionId);
            }

            return new ResponseModel()
            {
                Message = "Like successfully",
                Status = true
            };
        }

        public async Task<ResponseModel> Unlike(CommunityPostLikeModel communityPostLikeModel)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountById((Guid)communityPostLikeModel.UserId);
            if (account == null)
            {
                return new ResponseModel()
                {
                    Message = "User not found",
                    Status = false
                };
            }

            var post = await _unitOfWork.CommunityPostRepository.GetAsync((Guid)communityPostLikeModel.CommunityPostId);
            if (post == null)
            {
                return new ResponseModel()
                {
                    Message = "Post not found",
                    Status = false
                };
            }

            CommunityPostLike communityPostLike = await _unitOfWork.CommunityPostLikeRepository
                .GetByUserAndPost((Guid)communityPostLikeModel.UserId,(Guid)communityPostLikeModel.CommunityPostId);

            if(communityPostLike != null) 
            {
                _unitOfWork.CommunityPostLikeRepository.HardDelete(communityPostLike);
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
