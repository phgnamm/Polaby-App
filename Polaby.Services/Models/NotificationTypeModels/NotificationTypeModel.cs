using Polaby.Repositories.Enums;

namespace Polaby.Services.Models.NotificationTypeModels
{
    public class NotificationTypeModel
    {
        public Guid Id { get; set; }
        public NotificationTypeName? Name { get; set; }
        public string? Content { get; set; }
    }
}
