using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommunityPostModels;
using Polaby.Services.Models.ResponseModels;
using Polaby.Services.Models.ScheduleModels;

namespace Polaby.Services.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDataModel<ScheduleModel>> Create(ScheduleCreateModel scheduleCreateModel)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountById((Guid)scheduleCreateModel.UserId);
            if (account == null)
            {
                return new ResponseDataModel<ScheduleModel>()
                {
                    Message = "User not found",
                    Status = false
                };
            }

            Schedule schedule = _mapper.Map<Schedule>(scheduleCreateModel);

            await _unitOfWork.ScheduleRepository.AddAsync(schedule);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<ScheduleModel>(schedule);
            return new ResponseDataModel<ScheduleModel>()
            {
                Message = "Create schedule successfully!",
                Status = true,
                Data = result
            };
        }

        public async Task<ResponseDataModel<ScheduleModel>> Update(Guid id, ScheduleUpdateModel scheduleUpdateModel)
        {
            var existingSchedule = await _unitOfWork.ScheduleRepository.GetAsync(id);
            if (existingSchedule == null)
            {
                return new ResponseDataModel<ScheduleModel>()
                {
                    Message = "Schedule not found",
                    Status = false
                };
            }

            existingSchedule = _mapper.Map(scheduleUpdateModel, existingSchedule);
            _unitOfWork.ScheduleRepository.Update(existingSchedule);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<ScheduleModel>(existingSchedule);
            if (result != null)
            {
                return new ResponseDataModel<ScheduleModel>()
                {
                    Status = true,
                    Message = "Update post successfully",
                    Data = result
                };
            }
            return new ResponseDataModel<ScheduleModel>()
            {
                Status = false,
                Message = "Update post fail"
            };
        }

        public async Task<ResponseModel> Delete(Guid id)
        {
            var schedule = await _unitOfWork.ScheduleRepository.GetAsync(id);
            if (schedule != null)
            {
                var result = _mapper.Map<ScheduleModel>(schedule);
                _unitOfWork.ScheduleRepository.HardDelete(schedule);
                await _unitOfWork.SaveChangeAsync();
                if (result != null)
                {
                    return new ResponseModel()
                    {
                        Status = true,
                        Message = "Delete post successfully"
                    };
                }
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Delete post failed"
                };
            }
            return new ResponseModel()
            {
                Status = false,
                Message = "Post not found"
            };
        }

        public async Task<Pagination<ScheduleModel>> GetAllSchedules(ScheduleFilterModel scheduleFilterModel)
        {
            var scheduleList = await _unitOfWork.ScheduleRepository.GetAllAsync(
            filter: x =>
                x.IsDeleted == scheduleFilterModel.IsDeleted &&
                (scheduleFilterModel.UserId == null || x.UserId == scheduleFilterModel.UserId) &&
                (scheduleFilterModel.Date == null || x.Date == scheduleFilterModel.Date),

            orderBy: x =>
            {
                return scheduleFilterModel.OrderByDescending
                    ? x.OrderByDescending(x => x.CreationDate)
                    : x.OrderBy(x => x.CreationDate);
            },
            pageIndex: scheduleFilterModel.PageIndex,
            pageSize: scheduleFilterModel.PageSize,
            include: "User"
        );

            if (scheduleList != null)
            {
                var scheduleDetailList = scheduleList.Data.Select(cp => new ScheduleModel
                {
                    Id = cp.Id,
                    Title = cp.Title,
                    Location = cp.Location,
                    Note = cp.Note,
                    Date = cp.Date,
                    UserId = cp.UserId,
                    UserName = cp.User.FirstName + cp.User.FirstName
                }).ToList();

                return new Pagination<ScheduleModel>(scheduleDetailList, scheduleList.TotalCount, scheduleFilterModel.PageIndex, scheduleFilterModel.PageSize);
            }
            return null;
        }
    }
}
