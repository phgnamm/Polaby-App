using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories;

public class SubscriptionFormRepository: GenericRepository<SubscriptionForm>, ISubscriptionFormRepository
{
    public SubscriptionFormRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
    {
    }
}