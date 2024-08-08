using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommentModels;
using Polaby.Services.Models.CommunityPostModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDataModel<CommentModel>> Create(CommentCreateModel commentCreateModel)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountById((Guid)commentCreateModel.UserId);
            if (account == null)
            {
                return new ResponseDataModel<CommentModel>()
                {
                    Message = "User not found",
                    Status = false
                };
            }

            var communityPost = await _unitOfWork.CommunityPostRepository.GetAsync((Guid)commentCreateModel.PostId);
            if (communityPost == null)
            {
                return new ResponseDataModel<CommentModel>()
                {
                    Message = "Post not found",
                    Status = false
                };
            }

            if(commentCreateModel.ParentCommentId != null)
            {
                var parrentComment = await _unitOfWork.CommentRepository.GetAsync((Guid)commentCreateModel.ParentCommentId);
                if (parrentComment == null)
                {
                    return new ResponseDataModel<CommentModel>()
                    {
                        Message = "Parrent comment not found",
                        Status = false
                    };
                }
            }

            Comment comment = _mapper.Map<Comment>(commentCreateModel);

            await _unitOfWork.CommentRepository.AddAsync(comment);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<CommentModel>(comment);
            return new ResponseDataModel<CommentModel>()
            {
                Message = "Create comment successfully!",
                Status = true,
                Data = result
            };
        }

        public async Task<ResponseDataModel<CommentModel>> Update(Guid id, CommentUpdateModel commentUpdateModel)
        {
            var existingComment = await _unitOfWork.CommentRepository.GetAsync(id);
            if (existingComment == null)
            {
                return new ResponseDataModel<CommentModel>()
                {
                    Message = "Comment not found",
                    Status = false
                };
            }

            existingComment = _mapper.Map(commentUpdateModel, existingComment);
            _unitOfWork.CommentRepository.Update(existingComment);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<CommentModel>(existingComment);
            if (result != null)
            {
                return new ResponseDataModel<CommentModel>()
                {
                    Status = true,
                    Message = "Update comment successfully",
                    Data = result
                };
            }
            return new ResponseDataModel<CommentModel>()
            {
                Status = false,
                Message = "Update comment fail"
            };
        }

        public async Task<ResponseDataModel<CommentModel>> Delete(Guid id)
        {
            var comment = await _unitOfWork.CommentRepository.GetAsync(id);
            if (comment != null)
            {
                var result = _mapper.Map<CommentModel>(comment);
                _unitOfWork.CommentRepository.SoftDelete(comment);
                await _unitOfWork.SaveChangeAsync();
                if (result != null)
                {
                    return new ResponseDataModel<CommentModel>()
                    {
                        Status = true,
                        Message = "Delete comment successfully",
                        Data = result
                    };
                }
                return new ResponseDataModel<CommentModel>()
                {
                    Status = false,
                    Message = "Delete comment failed"
                };
            }
            return new ResponseDataModel<CommentModel>()
            {
                Status = false,
                Message = "Comment not found"
            };
        }

        public async Task<Pagination<CommentModel>> GetAllCommunityPosts(CommentFilterModel commentFilterModel)
        {
            var commentList = await _unitOfWork.CommentRepostiory.GetAllAsync(
            filter: x =>
                x.IsDeleted == commentFilterModel.IsDeleted &&
                (commentFilterModel.PostId == null || x.PostId == commentFilterModel.PostId) &&
                (commentFilterModel.ParentCommentId == null || x.ParentCommentId == commentFilterModel.ParentCommentId) &&
                (commentFilterModel.UserId == null || x.UserId == commentFilterModel.UserId) &&
                (string.IsNullOrEmpty(commentFilterModel.Search) ||
                 x.Content.ToLower().Contains(commentFilterModel.Search.ToLower())),

            orderBy: x =>
            {
                switch (commentFilterModel.Order.ToLower())
                {
                    case "like":
                        return commentFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.LikesCount)
                            : x.OrderBy(x => x.LikesCount);
                    case "report":
                        return commentFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.Reports.Count)
                            : x.OrderBy(x => x.Reports.Count);
                    default:
                        return commentFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.CreationDate)
                            : x.OrderBy(x => x.CreationDate);
                }
            },
            pageIndex: commentFilterModel.PageIndex,
            pageSize: commentFilterModel.PageSize,
            include: "Reports,Account,CommentLikes"

        );

            if (commentList != null)
            {
                var commentDetailList = commentList.Data.Select(cp => new CommentModel
                {
                    Id = cp.Id,
                    Content = cp.Content,
                    LikesCount = cp.LikesCount,
                    Attachments = cp.Attachments,
                    UserId = cp.UserId,
                    UserName = cp.Account.FirstName + cp.Account.FirstName,
                    ReportsCount = cp.Reports.Count,
                    IsLiked = cp.CommentLikes.Any()
                }).ToList();

                return new Pagination<CommentModel>(commentDetailList, commentList.TotalCount, commentFilterModel.PageIndex, commentFilterModel.PageSize);
            }
            return null;
        }
    }
}
