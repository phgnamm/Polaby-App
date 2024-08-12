using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class NotificationSettingRepository : GenericRepository<NotificationSetting>, INotificationSettingRepository
    {
        private readonly AppDbContext _dbContext;

        public NotificationSettingRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<NotificationSetting> GetByAccountAndType(Guid? accountId, Guid? notificationTypeId)
        {
            return await _dbContext.NotificationSetting.FirstOrDefaultAsync(ns => ns.AccountId == accountId && ns.NotificationTypeId == notificationTypeId);
        }
    }
}
