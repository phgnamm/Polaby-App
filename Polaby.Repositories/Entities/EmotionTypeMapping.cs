using Polaby.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polaby.Repositories.Entities
{
    public class EmotionTypeMapping : BaseEntity
    {
        public Guid? EmotionId { get; set; }  
        public EmotionType Type { get; set; }
        public Emotion? Emotion { get; set; }
    }
}
