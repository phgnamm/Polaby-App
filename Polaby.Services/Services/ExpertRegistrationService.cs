using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.ExpertRegistrationModels;
using Polaby.Repositories.Utils;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.ExpertRegistrationModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services;

public class ExpertRegistrationService : IExpertRegistrationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Account> _userManager;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public ExpertRegistrationService(IUnitOfWork unitOfWork, UserManager<Account> userManager, IMapper mapper,
        IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<ResponseModel> CreateExpertRegistration(
        ExpertRegistrationCreateModel expertRegistrationCreateModel)
    {
        var existedAccount = await _userManager.FindByEmailAsync(expertRegistrationCreateModel.Email);
        var existedRegistration =
            await _unitOfWork.ExpertRegistrationRepository.GetByEmail(expertRegistrationCreateModel.Email);

        if (existedAccount != null || existedRegistration != null)
        {
            return new ResponseModel
            {
                Status = false,
                Message = "Email already exists"
            };
        }

        var registration = _mapper.Map<ExpertRegistration>(expertRegistrationCreateModel);
        registration.Status = ExpertRegistrationStatus.Pending;
        await _unitOfWork.ExpertRegistrationRepository.AddAsync(registration);

        if (await _unitOfWork.SaveChangeAsync() > 0)
        {
            return new ResponseModel
            {
                Status = true,
                Message = "Submit registration successfully"
            };
        }

        return new ResponseModel
        {
            Status = false,
            Message = "Cannot submit registration"
        };
    }

    public async Task<ResponseDataModel<ExpertRegistrationModel>> GetExpertRegistration(Guid id)
    {
        var registration = await _unitOfWork.ExpertRegistrationRepository.GetAsync(id);

        if (registration == null)
        {
            return new ResponseDataModel<ExpertRegistrationModel>
            {
                Status = false,
                Message = "Weekly post not found"
            };
        }

        var registrationModel = _mapper.Map<ExpertRegistrationModel>(registration);

        return new ResponseDataModel<ExpertRegistrationModel>
        {
            Status = true,
            Message = "Get registration successfully",
            Data = registrationModel
        };
    }

    public async Task<ResponseDataModel<ExpertRegistrationModel>> GetExpertRegistration(string email)
    {
        var registration = await _unitOfWork.ExpertRegistrationRepository.GetByEmail(email);

        if (registration == null)
        {
            return new ResponseDataModel<ExpertRegistrationModel>
            {
                Status = false,
                Message = "Weekly post not found"
            };
        }

        var registrationModel = _mapper.Map<ExpertRegistrationModel>(registration);

        return new ResponseDataModel<ExpertRegistrationModel>
        {
            Status = true,
            Message = "Get registration successfully",
            Data = registrationModel
        };
    }

    public async Task<Pagination<ExpertRegistrationModel>> GetAllExpertRegistration(
        ExpertRegistrationFilterModel expertRegistrationFilterModel)
    {
        var registrations = await _unitOfWork.ExpertRegistrationRepository.GetAllAsync(
            pageIndex: expertRegistrationFilterModel.PageIndex,
            pageSize: expertRegistrationFilterModel.PageSize,
            filter: x =>
                x.IsDeleted == expertRegistrationFilterModel.IsDeleted &&
                (expertRegistrationFilterModel.MinYearsOfExperience == null ||
                 x.YearsOfExperience >= expertRegistrationFilterModel.MinYearsOfExperience) &&
                (expertRegistrationFilterModel.MaxYearsOfExperience == null ||
                 x.YearsOfExperience <= expertRegistrationFilterModel.MaxYearsOfExperience) &&
                (string.IsNullOrEmpty(expertRegistrationFilterModel.Search) ||
                 x.FirstName.ToLower().Contains(expertRegistrationFilterModel.Search) ||
                 x.LastName.ToLower().Contains(expertRegistrationFilterModel.Search) ||
                 x.Address.ToLower().Contains(expertRegistrationFilterModel.Search) ||
                 x.Email.ToLower().Contains(expertRegistrationFilterModel.Search) ||
                 x.ClinicAddress.ToLower().Contains(expertRegistrationFilterModel.Search) ||
                 x.Education.ToLower().Contains(expertRegistrationFilterModel.Search) ||
                 x.Workplace.ToLower().Contains(expertRegistrationFilterModel.Search) ||
                 x.Description.ToLower().Contains(expertRegistrationFilterModel.Search)),
            orderBy:
            (x =>
            {
                switch (expertRegistrationFilterModel.Order.ToLower())
                {
                    case "level":
                        return expertRegistrationFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.Level)
                            : x.OrderBy(x => x.Level);
                    case "status":
                        return expertRegistrationFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.Status)
                            : x.OrderBy(x => x.Status);
                    case "yearsofexperience":
                        return expertRegistrationFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.YearsOfExperience)
                            : x.OrderBy(x => x.YearsOfExperience);
                    default:
                        return expertRegistrationFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.CreationDate)
                            : x.OrderBy(x => x.CreationDate);
                }
            })
        );

        var registrationList = _mapper.Map<List<ExpertRegistrationModel>>(registrations.Data);
        return new Pagination<ExpertRegistrationModel>(registrationList, registrations.TotalCount,
            expertRegistrationFilterModel.PageIndex,
            expertRegistrationFilterModel.PageSize);
    }

    public async Task<ResponseModel> UpdateExpertRegistration(Guid id,
        ExpertRegistrationUpdateModel expertRegistrationUpdateModel)
    {
        var registration = await _unitOfWork.ExpertRegistrationRepository.GetAsync(id);

        if (registration == null)
        {
            return new ResponseModel
            {
                Status = false,
                Message = "Registration not found"
            };
        }

        if (registration.Email != expertRegistrationUpdateModel.Email)
        {
            var existedAccount = await _userManager.FindByEmailAsync(expertRegistrationUpdateModel.Email);
            var existedRegistration =
                await _unitOfWork.ExpertRegistrationRepository.GetByEmail(expertRegistrationUpdateModel.Email);

            if (existedAccount != null || existedRegistration != null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Email already exists"
                };
            }
        }

        registration.FirstName = expertRegistrationUpdateModel.FirstName;
        registration.LastName = expertRegistrationUpdateModel.LastName;
        registration.Gender = expertRegistrationUpdateModel.Gender;
        registration.DateOfBirth = expertRegistrationUpdateModel.DateOfBirth;
        registration.Address = expertRegistrationUpdateModel.Address;
        registration.PhoneNumber = expertRegistrationUpdateModel.PhoneNumber;
        registration.CertificateUrl = expertRegistrationUpdateModel.CertificateUrl;
        registration.ClinicLicenseUrl = expertRegistrationUpdateModel.ClinicLicenseUrl;
        registration.CCCDBackUrl = expertRegistrationUpdateModel.CCCDBackUrl;
        registration.CCCDFrontUrl = expertRegistrationUpdateModel.CCCDFrontUrl;
        registration.ClinicAddress = expertRegistrationUpdateModel.ClinicAddress;
        registration.Description = expertRegistrationUpdateModel.Description;
        registration.Education = expertRegistrationUpdateModel.Education;
        registration.YearsOfExperience = expertRegistrationUpdateModel.YearsOfExperience;
        registration.Workplace = expertRegistrationUpdateModel.Workplace;
        registration.Level = expertRegistrationUpdateModel.Level;
        _unitOfWork.ExpertRegistrationRepository.Update(registration);

        if (await _unitOfWork.SaveChangeAsync() > 0)
        {
            return new ResponseModel
            {
                Status = true,
                Message = "Update registration successfully"
            };
        }

        return new ResponseModel
        {
            Status = false,
            Message = "Cannot update registration"
        };
    }

    public async Task<ResponseModel> DeleteExpertRegistration(Guid id)
    {
        var registration = await _unitOfWork.ExpertRegistrationRepository.GetAsync(id);

        if (registration == null)
        {
            return new ResponseModel
            {
                Status = false,
                Message = "Registration not found"
            };
        }

        _unitOfWork.ExpertRegistrationRepository.HardDelete(registration);

        if (await _unitOfWork.SaveChangeAsync() > 0)
        {
            return new ResponseModel
            {
                Status = true,
                Message = "Delete registration successfully"
            };
        }

        return new ResponseModel
        {
            Status = false,
            Message = "Cannot delete registration"
        };
    }

    public async Task<ResponseModel> UpdateExpertRegistrationStatus(Guid id,
        ExpertRegistrationUpdateStatusModel expertRegistrationUpdateStatusModel)
    {
        var registration = await _unitOfWork.ExpertRegistrationRepository.GetAsync(id);

        if (registration == null)
        {
            return new ResponseModel
            {
                Status = false,
                Message = "Registration not found"
            };
        }

        if (expertRegistrationUpdateStatusModel.Status == ExpertRegistrationStatus.Rejected)
        {
            await _emailService.SendEmailAsync(registration.Email!, "Polaby - Đơn đăng ký của bạn đã được xem xét",
                $"Đơn đăng ký của bạn đã bị từ chối, nếu bạn nghĩ đây là một sai sót, vui lòng kiểm tra lại đơn đăng ký của bạn và nộp lại một lần nữa. Ghi chú: {expertRegistrationUpdateStatusModel.Note}",
                true);
        }
        
        if (expertRegistrationUpdateStatusModel.Status == ExpertRegistrationStatus.Approved)
        {
            // Create new account
            var user = _mapper.Map<Account>(registration);
            user.UserName = user.Email;
            user.CreationDate = DateTime.Now;
            // user.VerificationCode = AuthenticationTools.GenerateVerificationCode(6);
            // user.VerificationCodeExpiryTime = DateTime.Now.AddMinutes(15);
            
            // thông báo tạo thành công, tái sử dụng lại api forget pw và reset pw
            
            await _emailService.SendEmailAsync(registration.Email!, "Polaby - Đơn đăng ký của bạn đã được xem xét",
                $"Đơn đăng ký của bạn đã bị từ chối, nếu bạn nghĩ đây là một sai sót, vui lòng kiểm tra lại đơn đăng ký của bạn và nộp lại một lần nữa. Ghi chú: {expertRegistrationUpdateStatusModel.Note}",
                true);
        }
        
        if (await _unitOfWork.SaveChangeAsync() > 0)
        {
            return new ResponseModel
            {
                Status = true,
                Message = "Update registration successfully"
            };
        }

        return new ResponseModel
        {
            Status = false,
            Message = "Cannot update registration"
        };
    }
}