using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.AccountModels;
using Polaby.Services.Models.CommunityPostModels;
using Polaby.Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Services
{
    public class CommunityPostService : ICommunityPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommunityPostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDataModel<CommunityPostModel>> Create(CommunityPostCreateModel communityPostCreateModel)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountById((Guid)communityPostCreateModel.UserId);
            if (account == null)
            {
                return new ResponseDataModel<CommunityPostModel>()
                {
                    Message = "User not found",
                    Status = false
                };
            }

            CommunityPost communityPost = _mapper.Map<CommunityPost>(communityPostCreateModel);

            await _unitOfWork.CommunityPostRepository.AddAsync(communityPost);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<CommunityPostModel>(communityPost);
            return new ResponseDataModel<CommunityPostModel>()
            {
                Message = "Create post successfully!",
                Status = true,
                Data = result
            };
        }

        public async Task<ResponseDataModel<CommunityPostModel>> Update(Guid id, CommunityPostUpdateModel communityPostUpdateModel)
        {
            var existingCommunityPost = await _unitOfWork.CommunityPostRepository.GetAsync(id);
            if (existingCommunityPost == null)
            {
                return new ResponseDataModel<CommunityPostModel>()
                {
                    Message = "Post not found",
                    Status = false
                };
            }

            existingCommunityPost = _mapper.Map(communityPostUpdateModel, existingCommunityPost);
            _unitOfWork.CommunityPostRepository.Update(existingCommunityPost);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<CommunityPostModel>(existingCommunityPost);
            if (result != null)
            {
                return new ResponseDataModel<CommunityPostModel>()
                {
                    Status = true,
                    Message = "Update post successfully",
                    Data = result
                };
            }
            return new ResponseDataModel<CommunityPostModel>()
            {
                Status = false,
                Message = "Update post fail"
            };
        }

        public async Task<ResponseDataModel<CommunityPostModel>> Delete(Guid id)
        {
            var communityPost = await _unitOfWork.CommunityPostRepository.GetAsync(id);
            if (communityPost != null)
            {
                var result = _mapper.Map<CommunityPostModel>(communityPost);
                _unitOfWork.CommunityPostRepository.SoftDelete(communityPost);
                await _unitOfWork.SaveChangeAsync();
                if (result != null)
                {
                    return new ResponseDataModel<CommunityPostModel>()
                    {
                        Status = true,
                        Message = "Delete post successfully",
                        Data = result
                    };
                }
                return new ResponseDataModel<CommunityPostModel>()
                {
                    Status = false,
                    Message = "Delete post failed"
                };
            }
            return new ResponseDataModel<CommunityPostModel>()
            {
                Status = false,
                Message = "Post not found"
            };
        }

        public async Task<Pagination<CommunityPostModel>> GetAllCommunityPosts(CommunityPostFilterModel communityPostFilterModel)
        {
            var communityPostList = await _unitOfWork.CommuntityPostRepository.GetAllAsync(
            filter: x =>
                x.IsDeleted == communityPostFilterModel.IsDeleted &&
                (communityPostFilterModel.IsProfessional == null || x.IsProfessional == communityPostFilterModel.IsProfessional) &&
                (communityPostFilterModel.Visibility == null || x.Visibility == communityPostFilterModel.Visibility) &&
                (communityPostFilterModel.UserId == null || x.UserId == communityPostFilterModel.UserId) &&
                (string.IsNullOrEmpty(communityPostFilterModel.Search) ||
                 x.Title.ToLower().Contains(communityPostFilterModel.Search.ToLower()) ||
                 x.Content.ToLower().Contains(communityPostFilterModel.Search.ToLower())),

            orderBy: x =>
            {
                switch (communityPostFilterModel.Order.ToLower())
                {
                    case "like":
                        return communityPostFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.LikesCount)
                            : x.OrderBy(x => x.LikesCount);
                    case "comment":
                        return communityPostFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.CommentsCount)
                            : x.OrderBy(x => x.CommentsCount);
                    default:
                        return communityPostFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.CreationDate)
                            : x.OrderBy(x => x.CreationDate);
                }
            },
            pageIndex: communityPostFilterModel.PageIndex,
            pageSize: communityPostFilterModel.PageSize,
            include: "Reports,Account,CommunityPostLikes"

        );

            if (communityPostList != null)
            {
                var communityPostDetailList = communityPostList.Data.Select(cp => new CommunityPostModel
                {
                    Id = cp.Id,
                    Title = cp.Title,
                    Content = cp.Content,
                    LikesCount = cp.LikesCount,
                    CommentsCount = cp.CommentsCount,
                    ImageUrl = cp.ImageUrl,
                    Attachments = cp.Attachments,
                    IsProfessional = cp.IsProfessional,
                    Visibility = cp.Visibility,
                    UserId = cp.UserId,
                    UserName = cp.Account.FirstName + cp.Account.FirstName,
                    ReportsCount = cp.Reports.Count,
                    IsLiked = cp.CommunityPostLikes.Any()
                }).ToList();

                return new Pagination<CommunityPostModel>(communityPostDetailList, communityPostList.TotalCount, communityPostFilterModel.PageIndex, communityPostFilterModel.PageSize);
            }
            return null;
        }
    }
}
