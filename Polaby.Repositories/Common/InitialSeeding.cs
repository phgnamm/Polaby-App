using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;
using Role = Polaby.Repositories.Entities.Role;

namespace Polaby.Repositories.Common
{
    /// <summary>
    /// This class is used to insert initial data
    /// </summary>
    public class InitialSeeding
    {
        private static readonly string[] RoleList =
            [Enums.Role.Admin.ToString(), Enums.Role.User.ToString(), Enums.Role.Expert.ToString()];

        private static readonly List<NotificationType> NotificationTypes =
        [
            new NotificationType { Name = NotificationTypeName.Like, Content = "đã thích bài viết của bạn." },
            new NotificationType { Name = NotificationTypeName.Comment, Content = "đã bình luận bài viết của bạn." },
            new NotificationType { Name = NotificationTypeName.Follow, Content = "đã bắt đầu theo dõi bạn." },
            new NotificationType { Name = NotificationTypeName.Rate, Content = "đã thêm đánh giá trang của bạn." },
        ];

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            foreach (string role in RoleList)
            {
                Role? existedRole = await roleManager.FindByNameAsync(role);
                if (existedRole == null)
                {
                    await roleManager.CreateAsync(new Role { Name = role });
                }
            }

            await context.Database.MigrateAsync();

            foreach (var type in NotificationTypes)
            {
                if (!context.NotificationType.Any(x => x.Name == type.Name))
                {
                    type.CreationDate = DateTime.Now;
                    context.NotificationType.Add(type);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}