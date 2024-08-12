namespace Polaby.Services.Models.NotificationModels
{
    public class NotificationSettingUpdateModel
    {
        public Guid? Id { get; set; }
        public bool IsEnabled { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? NotificationTypeId { get; set; }
    }
}
