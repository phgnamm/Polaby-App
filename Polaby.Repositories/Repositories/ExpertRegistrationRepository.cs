using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories;

public class ExpertRegistrationRepository : GenericRepository<ExpertRegistration>, IExpertRegistrationRepository
{
    public ExpertRegistrationRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
    {
    }

    public async Task<ExpertRegistration?> GetByEmail(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
    }
}