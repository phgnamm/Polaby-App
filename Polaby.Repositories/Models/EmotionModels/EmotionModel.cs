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
        public DateOnly Date { get; set; }
        public Guid UserId { get; set; }
        public List<EmotionTypeDto> EmotionTypes { get; set; }
        public List<NoteEmotionDto> Notes { get; set; }
    }

    public class EmotionTypeDto
    {
        public EmotionType EmotionType { get; set; }
    }

    public class NoteEmotionDto
    {
        public string Content { get; set; }
        public bool IsSelected { get; set; }
    }
}
