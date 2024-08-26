using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommunityPostLikeModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services
{
    public class CommunityPostLikeService : ICommunityPostLikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommunityPostLikeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            await _unitOfWork.SaveChangeAsync();

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
