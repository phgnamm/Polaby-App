using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.HealthModels;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.HealthModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services
{
    public class HealthService : IHealthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HealthService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> AddHealthAsync(List<HealthCreateModel> healthModels)
        {
            var healthEntities = _mapper.Map<List<Health>>(healthModels)
                    .Select(entity =>
                    {
                        if (entity.Date == default)
                        {
                            entity.Date = DateOnly.FromDateTime(DateTime.Now);
                        }
                        return entity;
                    }).ToList();


            if (healthEntities == null || !healthEntities.Any())
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Cannot create Health records!"
                };
            }

            var userIds = healthEntities.Select(h => h.UserId.Value).Distinct().ToList();
            var types = healthEntities.Select(h => h.Type).Distinct().ToList();
            var dates = healthEntities.Select(h => h.Date).Distinct().ToList();

            var existingHealthResult = await _unitOfWork.HealthRepository.GetAllAsync(
                filter: h => userIds.Contains(h.UserId.Value)
                    && types.Contains(h.Type)
                    && dates.Contains(h.Date)
            );

            var existingHealths = existingHealthResult.Data;
            if (existingHealths.Any())
            {
                var duplicate = existingHealths.First();
                return new ResponseModel
                {
                    Status = false,
                    Message = $"Health for {duplicate.Type} on {duplicate.Date} already exists!"
                };
            }

            await _unitOfWork.HealthRepository.AddRangeAsync(healthEntities);
            await _unitOfWork.SaveChangeAsync();
            return new ResponseModel
            {
                Status = true,
                Message = "Health created successfully"
            };
        }



        public async Task<Pagination<HealthModel>> GetAllHealthAsync(HealthFilterModel filterModel)
        {
            var healthList = await _unitOfWork.HealthRepository.GetAllAsync(
                pageIndex: filterModel.PageIndex,
                pageSize: filterModel.PageSize,
                filter: x =>
                    x.IsDeleted == filterModel.IsDeleted &&
                    (filterModel.UserId == null || x.UserId == filterModel.UserId) &&
                    (filterModel.Date == default || x.Date == filterModel.Date) &&
                    (string.IsNullOrEmpty(filterModel.Search) ||
                     x.Type.ToString().ToLower().Contains(filterModel.Search.ToLower())) &&
                    (filterModel.FilterWeight == false || x.Type == HealthType.Weight) &&
                    (filterModel.FilterHeight == false || x.Type == HealthType.Height) &&
                    (filterModel.FilterSize == false || x.Type == HealthType.Size) &&
                    (filterModel.FilterBloodPressureSys == false || x.Type == HealthType.BloodPressureSys) &&
                    (filterModel.FilterBloodPressureDia == false || x.Type == HealthType.BloodPressureDia) &&
                    (filterModel.FilterHeartbeat == false || x.Type == HealthType.Heartbeat) &&
                    (filterModel.FilterContractility == false || x.Type == HealthType.Contractility),
                orderBy: x =>
                {
                    switch (filterModel.Order.ToLower())
                    {
                        case "type":
                            return filterModel.OrderByDescending
                                ? x.OrderByDescending(h => h.Type)
                                : x.OrderBy(h => h.Type);
                        case "date":
                            return filterModel.OrderByDescending
                                ? x.OrderByDescending(h => h.Date)
                                : x.OrderBy(h => h.Date);
                        default:
                            return filterModel.OrderByDescending
                                ? x.OrderByDescending(h => h.CreationDate)
                                : x.OrderBy(h => h.CreationDate);
                    }
                }
            );

            if (healthList != null)
            {
                var healthModelList = _mapper.Map<List<HealthModel>>(healthList.Data);
                return new Pagination<HealthModel>(healthModelList, healthList.TotalCount, filterModel.PageIndex, filterModel.PageSize);
            }
            return null;
        }



        public async Task<ResponseModel> UpdateHealthAsync(Guid id, HealthUpdateModel updateModel)
        {
            var existingHealth = await _unitOfWork.HealthRepository.GetAsync(id);
            if (existingHealth == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Health not found!"
                };
            }

            _mapper.Map(updateModel, existingHealth);
            _unitOfWork.HealthRepository.Update(existingHealth);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel
            {
                Status = true,
                Message = "Health updated successfully"
            };
        }

        public async Task<ResponseModel> DeleteHealthAsync(Guid id)
        {
            var existingHealth = await _unitOfWork.HealthRepository.GetAsync(id);
            if (existingHealth == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Health not found!"
                };
            }

            _unitOfWork.HealthRepository.HardDelete(existingHealth);
            await _unitOfWork.SaveChangeAsync();
            return new ResponseModel
            {
                Status = true,
                Message = "Health deleted successfully"
            };
        }
    }
}
