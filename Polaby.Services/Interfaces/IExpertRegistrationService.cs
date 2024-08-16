using Polaby.Repositories.Models.ExpertRegistrationModels;
using Polaby.Services.Common;
using Polaby.Services.Models.ExpertRegistrationModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces;

public interface IExpertRegistrationService
{
    Task<ResponseModel> CreateExpertRegistration(ExpertRegistrationCreateModel expertRegistrationCreateModel);
    Task<ResponseDataModel<ExpertRegistrationModel>> GetExpertRegistration(Guid id);
    Task<ResponseDataModel<ExpertRegistrationModel>> GetExpertRegistration(string email);

    Task<Pagination<ExpertRegistrationModel>> GetAllExpertRegistration(
        ExpertRegistrationFilterModel expertRegistrationFilterModel);
    Task<ResponseModel> UpdateExpertRegistration(Guid id, ExpertRegistrationUpdateModel expertRegistrationUpdateModel);
    Task<ResponseModel> DeleteExpertRegistration(Guid id);
    Task<ResponseModel> UpdateExpertRegistrationStatus(Guid id, ExpertRegistrationUpdateStatusModel expertRegistrationUpdateStatusModel);
}