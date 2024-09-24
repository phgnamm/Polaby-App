using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class NotificationTypeRepository : GenericRepository<NotificationType>, INotificationTypeRepository
    {
        private readonly AppDbContext _dbContext;

        public NotificationTypeRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<NotificationType> GetByName (NotificationTypeName name)
        {
            return await _dbContext.NotificationType.FirstOrDefaultAsync(nt => nt.Name == name);
        }
    }
}
