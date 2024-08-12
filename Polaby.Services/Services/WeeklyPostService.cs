using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.WeeklyPostModels;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.ResponseModels;
using Polaby.Services.Models.WeeklyPostModels;

namespace Polaby.Services.Services;

public class WeeklyPostService : IWeeklyPostService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WeeklyPostService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel> AddWeeklyPosts(List<WeeklyPostCreateModel> weeklyPostCreateModels)
    {
        // Handle duplicate weeks in inputted list
        var weekList = weeklyPostCreateModels.Select(model => model.Week).ToList();
        var hasDuplicates = weekList.Count != weekList.Distinct().Count();

        // Handle duplicate weeks in database
        var validWeeks = await _unitOfWork.WeeklyPostRepository.GetValidWeeks(weekList);

        if (hasDuplicates || validWeeks == null)
        {
            return new ResponseModel
            {
                Status = false,
                Message = "Duplicate week values detected"
            };
        }

        weeklyPostCreateModels = weeklyPostCreateModels.Where(x => validWeeks.Contains(x.Week)).ToList();
        await _unitOfWork.WeeklyPostRepository.AddRangeAsync(_mapper.Map<List<WeeklyPost>>(weeklyPostCreateModels));
        var result = await _unitOfWork.SaveChangeAsync();

        if (result > 0)
        {
            return new ResponseModel
            {
                Status = true,
                Message = $"Add {result} weekly posts successfully"
            };
        }

        return new ResponseModel
        {
            Status = false,
            Message = "Cannot add weekly posts",
        };
    }

    public async Task<ResponseDataModel<WeeklyPostModel>> GetWeeklyPost(int week)
    {
        var post = await _unitOfWork.WeeklyPostRepository.GetPostByWeek(week);

        if (post == null)
        {
            return new ResponseDataModel<WeeklyPostModel>
            {
                Status = false,
                Message = "Weekly post not found"
            };
        }

        var postModel = _mapper.Map<WeeklyPostModel>(post);

        return new ResponseDataModel<WeeklyPostModel>
        {
            Status = true,
            Message = "Get weekly post successfully",
            Data = postModel
        };
    }

    public async Task<ResponseDataModel<WeeklyPostModel>> GetWeeklyPost(Guid id)
    {
        var post = await _unitOfWork.WeeklyPostRepository.GetAsync(id);

        if (post == null)
        {
            return new ResponseDataModel<WeeklyPostModel>
            {
                Status = false,
                Message = "Weekly post not found"
            };
        }

        var postModel = _mapper.Map<WeeklyPostModel>(post);

        return new ResponseDataModel<WeeklyPostModel>
        {
            Status = true,
            Message = "Get weekly post successfully",
            Data = postModel
        };
    }

    public async Task<Pagination<WeeklyPostModel>> GetAllWeeklyPosts(WeeklyPostFilterModel weeklyPostFilterModel)
    {
        var postList = await _unitOfWork.WeeklyPostRepository.GetAllAsync(pageIndex: weeklyPostFilterModel.PageIndex,
            pageSize: weeklyPostFilterModel.PageSize,
            filter: x =>
                x.IsDeleted == weeklyPostFilterModel.IsDeleted &&
                (string.IsNullOrEmpty(weeklyPostFilterModel.Search) ||
                 x.AboutMother.ToLower().Contains(weeklyPostFilterModel.Search) ||
                 x.AboutBaby.ToLower().Contains(weeklyPostFilterModel.Search) ||
                 x.Advice.ToLower().Contains(weeklyPostFilterModel.Search) ||
                 x.Size.ToString().ToLower().Contains(weeklyPostFilterModel.Search) ||
                 x.Weight.ToString().ToLower().Contains(weeklyPostFilterModel.Search) ||
                 x.Week.ToString().ToLower().Contains(weeklyPostFilterModel.Search)),
            orderBy:
            (x =>
            {
                switch (weeklyPostFilterModel.Order.ToLower())
                {
                    case "week":
                        return weeklyPostFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.Week)
                            : x.OrderBy(x => x.Week);
                    case "size":
                        return weeklyPostFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.Size)
                            : x.OrderBy(x => x.Size);
                    case "weight":
                        return weeklyPostFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.Weight)
                            : x.OrderBy(x => x.Weight);
                    default:
                        return weeklyPostFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.CreationDate)
                            : x.OrderBy(x => x.CreationDate);
                }
            })
        );

        var postModelList = _mapper.Map<List<WeeklyPostModel>>(postList.Data);
        return new Pagination<WeeklyPostModel>(postModelList, postList.TotalCount,
            weeklyPostFilterModel.PageIndex,
            weeklyPostFilterModel.PageSize);
    }

    public async Task<ResponseModel> UpdateWeeklyPost(Guid id, WeeklyPostUpdateModel weeklyPostUpdateModel)
    {
        var post = await _unitOfWork.WeeklyPostRepository.GetAsync(id);

        if (post == null)
        {
            return new ResponseModel
            {
                Status = false,
                Message = "Weekly post not found"
            };
        }

        if (post.Week != weeklyPostUpdateModel.Week)
        {
            var isExistedWeek = await _unitOfWork.WeeklyPostRepository.GetPostByWeek(weeklyPostUpdateModel.Week) !=
                                null;

            if (isExistedWeek)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "New week number already exists"
                };
            }
        }

        post.Week = weeklyPostUpdateModel.Week;
        post.AboutBaby = weeklyPostUpdateModel.AboutBaby;
        post.AboutMother = weeklyPostUpdateModel.AboutMother;
        post.Weight = weeklyPostUpdateModel.Weight;
        post.Size = weeklyPostUpdateModel.Size;
        post.Source = weeklyPostUpdateModel.Source;
        post.SourceUrl = weeklyPostUpdateModel.SourceUrl;
        _unitOfWork.WeeklyPostRepository.Update(post);

        if (await _unitOfWork.SaveChangeAsync() > 0)
        {
            return new ResponseModel
            {
                Status = true,
                Message = "Update weekly post successfully"
            };
        }

        return new ResponseDataModel<WeeklyPostModel>
        {
            Status = false,
            Message = "Cannot update weekly post"
        };
    }

    public async Task<ResponseModel> DeleteWeeklyPosts(Guid id)
    {
        var post = await _unitOfWork.WeeklyPostRepository.GetAsync(id);

        if (post == null)
        {
            return new ResponseModel
            {
                Status = false,
                Message = "Weekly post not found"
            };
        }

        _unitOfWork.WeeklyPostRepository.HardDelete(post);

        if (await _unitOfWork.SaveChangeAsync() > 0)
        {
            return new ResponseModel
            {
                Status = true,
                Message = "Delete weekly post successfully"
            };
        }

        return new ResponseDataModel<WeeklyPostModel>
        {
            Status = false,
            Message = "Cannot delete weekly post"
        };
    }
}