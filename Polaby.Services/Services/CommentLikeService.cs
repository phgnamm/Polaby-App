using AutoMapper;
using Polaby.Repositories.Entities;
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

        public CommentLikeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
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
