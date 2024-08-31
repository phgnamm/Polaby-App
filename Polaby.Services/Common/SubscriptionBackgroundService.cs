using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polaby.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Common
{
    public class SubscriptionBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public SubscriptionBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async  Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckSubscriptionsAsync();
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
        private async Task CheckSubscriptionsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var accountsToUpdate = dbContext.Users
                    .Where(a => a.SubscriptionEndDate <= DateTime.Now && a.IsSubscriptionActive)
                    .ToList();

                foreach (var account in accountsToUpdate)
                {
                    account.IsSubscriptionActive = false;
                }

                if (accountsToUpdate.Any())
                {
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
