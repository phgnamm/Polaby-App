using Polaby.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Models.EmotionModels
{
    public class EmotionFilterModel: PaginationParameter
    {
        public Guid? UserId { get; set; }
        public DateOnly? Date { get; set; }
    }
}
