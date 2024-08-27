using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.SafeFoodModels;
using Polaby.Services.Models.ResponseModels;
using Polaby.Repositories.Models.SafeFoodModels;

namespace Polaby.Services.Services
{
    public class SafeFoodService : ISafeFoodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SafeFoodService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> AddSafeFood(SafeFoodCreateModel createModel)
        {
            var safeFood = _mapper.Map<SafeFood>(createModel);

            if (safeFood == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Cannot create SafeFood!"
                };
            }

            await _unitOfWork.SafeFoodRepository.AddAsync(safeFood);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "SafeFood created successfully"
            };
        }

        public async Task<Pagination<SafeFoodModel>> GetAllSafeFoods(SafeFoodFilterModel filterModel)
        {
            var safeFoodList = await _unitOfWork.SafeFoodRepository.GetAllAsync(
                pageIndex: filterModel.PageIndex,
                pageSize: filterModel.PageSize,
                filter: (x =>
                    !x.IsDeleted &&
                     x.IsSafe == filterModel.IsSafe &&
                    (string.IsNullOrEmpty(filterModel.Search) ||
                     x.Name.ToLower().Contains(filterModel.Search.ToLower()) ||
                     x.Description.ToLower().Contains(filterModel.Search.ToLower()))
                ),
                orderBy: x =>
                {
                    switch (filterModel.Order.ToLower())
                    {
                        case "name":
                            return filterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.Name)
                                : x.OrderBy(x => x.Name);
                        case "issafe":
                            return filterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.IsSafe)
                                : x.OrderBy(x => x.IsSafe);
                        default:
                            return filterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.CreationDate)
                                : x.OrderBy(x => x.CreationDate);
                    }
                }
            );

            if (safeFoodList != null)
            {
                var safeFoodModelList = _mapper.Map<List<SafeFoodModel>>(safeFoodList.Data);
                return new Pagination<SafeFoodModel>(safeFoodModelList, filterModel.PageIndex, filterModel.PageSize, safeFoodList.TotalCount);
            }
            return null;
        }


        public async Task<ResponseModel> UpdateSafeFood(Guid id, SafeFoodCreateModel updateModel)
        {
            var existingSafeFood = await _unitOfWork.SafeFoodRepository.GetAsync(id);
            if (existingSafeFood == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "SafeFood not found!"
                };
            }

            _mapper.Map(updateModel, existingSafeFood);
            _unitOfWork.SafeFoodRepository.Update(existingSafeFood);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "SafeFood updated successfully"
            };
        }

        public async Task<ResponseModel> DeleteSafeFood(Guid id)
        {
            var existingSafeFood = await _unitOfWork.SafeFoodRepository.GetAsync(id);
            if (existingSafeFood == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "SafeFood not found!"
                };
            }

            _unitOfWork.SafeFoodRepository.HardDelete(existingSafeFood);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "SafeFood deleted successfully"
            };
        }

        public async Task<ResponseDataModel<SafeFoodModel>> GetById(Guid id)
        {
            var safeFood = await _unitOfWork.SafeFoodRepository.GetAsync(id);

            if (safeFood == null)
            {
                return new ResponseDataModel<SafeFoodModel>()
                {
                    Status = false,
                    Message = "Safe food not found"
                };
            }

            var safeFoodModel = _mapper.Map<SafeFoodModel>(safeFood);

            return new ResponseDataModel<SafeFoodModel>()
            {
                Status = true,
                Message = "Get safe food successfully",
                Data = safeFoodModel
            };
        }
    }
}
