using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.ResponseModels;
using Polaby.Services.Models.SubscriptionFormModels;

namespace Polaby.Services.Services;

public class SubscriptionFormService : ISubscriptionFormService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubscriptionFormService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel> CreateSubscriptionForm(SubscriptionFormCreateModel createSubscriptionFormCreateModel)
    {
        var form = _mapper.Map<SubscriptionForm>(createSubscriptionFormCreateModel);
        await _unitOfWork.SubscriptionFormRepository.AddAsync(form);
        if (await _unitOfWork.SaveChangeAsync() > 0)
        {
            return new ResponseModel
            {
                Status = true,
                Message = "Subscription form created successfully.",
            };
        }

        return new ResponseModel()
        {
            Status = false,
            Message = "Failed to create subscription form.",
        };
    }
}