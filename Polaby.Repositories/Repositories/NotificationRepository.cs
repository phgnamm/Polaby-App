using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly AppDbContext _dbContext;

        public NotificationRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<Notification> GetById(Guid id)
        {
            return await _dbContext.Notification
                .Include(cp => cp.Post)
                .Include(cp => cp.Receiver)
                .Include(cp => cp.Sender)
                .Include(cp => cp.NotificationType)
                .FirstOrDefaultAsync(cp => cp.Id == id);
        }
    }
}
