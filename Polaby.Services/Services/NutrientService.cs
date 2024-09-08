

using AutoMapper;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Interfaces;

namespace Polaby.Services.Services
{
    public class NutrientService : INutrientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;

        public NutrientService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
        }
    }
}
