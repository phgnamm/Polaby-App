using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Models.EmotionModels
{
    public class EmotionModel : BaseEntity
    {
        public EmotionType? Type { get; set; }
        public DateOnly? Date { get; set; }
        public Guid? UserId { get; set; }
    }
}
