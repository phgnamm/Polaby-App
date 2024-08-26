using Polaby.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Models.EmotionModels
{
    public class EmotionRequestModel
    {
        public Guid UserId { get; set; }
        public EmotionType Type { get; set; }
    }
}
