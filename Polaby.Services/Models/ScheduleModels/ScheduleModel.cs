using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Models.ScheduleModels
{
    public class ScheduleModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Location { get; set; }
        public string? Note { get; set; }
        public DateOnly? Date { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
    }
}
