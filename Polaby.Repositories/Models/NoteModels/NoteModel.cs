using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Models.NoteModels
{
    public class NoteModel : BaseEntity
    {
        public string? Title { get; set; }
        public Trimester Trimester { get; set; }
        public DateOnly? Date { get; set; }
        public Guid? UserId { get; set; }
    }
}
