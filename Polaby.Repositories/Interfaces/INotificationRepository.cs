using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<Notification> GetById(Guid id);
    }
}
