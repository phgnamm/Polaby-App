using Polaby.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Models.NotificationModels
{
    public class NotificationSettingModel:BaseEntity
    {
        public Guid Id { get; set; }
        public bool IsEnabled { get; set; }
        public Guid? AccountId { get; set; }
        public string AccountName { get; set; }
        public Guid? NotificationTypeId { get; set; }
        public string NotificationTypeName { get; set; }
        public string NotificationTypeContent { get; set; }
    }
}