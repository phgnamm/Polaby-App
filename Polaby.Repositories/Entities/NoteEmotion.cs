using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Polaby.Repositories.Entities
{
    public class NoteEmotion : BaseEntity
    {
        public Guid? EmotionId { get; set; }  
        public string Content { get; set; }
        public bool IsSelected { get; set; }

        // Navigation property
        public virtual Emotion? Emotion { get; set; }
    }
}
