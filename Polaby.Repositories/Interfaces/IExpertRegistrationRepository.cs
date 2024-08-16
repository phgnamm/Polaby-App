using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces;

public interface IExpertRegistrationRepository : IGenericRepository<ExpertRegistration>
{
    Task<ExpertRegistration?> GetByEmail(string email);
}