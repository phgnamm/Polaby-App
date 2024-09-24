using Polaby.Services.Models.ResponseModels;
using Polaby.Services.Models.SubscriptionFormModels;

namespace Polaby.Services.Interfaces;

public interface ISubscriptionFormService
{
    Task<ResponseModel> CreateSubscriptionForm(SubscriptionFormCreateModel createSubscriptionFormCreateModel);
}