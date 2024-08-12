using AutoMapper;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Interfaces;

namespace Polaby.Services.Services
{
    public class NotificationTypeService : INotificationTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
