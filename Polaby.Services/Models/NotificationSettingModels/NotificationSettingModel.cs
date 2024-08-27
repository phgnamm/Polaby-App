namespace Polaby.Services.Models.NotificationModels
{
    public class NotificationSettingModel
    {
        public Guid Id { get; set; }
        public bool IsEnabled { get; set; }
        public Guid? AccountId { get; set; }
        public string AccountName { get; set; }
        public Guid? NotificationTypeId { get; set; }
        public string NotificationTypeName { get; set; }
    }
}