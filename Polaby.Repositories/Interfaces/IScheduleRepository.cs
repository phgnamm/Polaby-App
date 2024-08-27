using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        Task<Schedule> GetById(Guid id);
    }
}
