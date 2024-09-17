using Polaby.Repositories.Common;
using Polaby.Repositories.Enums;
using Polaby.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Models.NoteModels
{
    public class NoteFilterModel: PaginationParameter
    {
        public Guid? UserId { get; set; }
        public DateOnly? Date { get; set; }
        public Trimester? Trimester { get; set; }
        public string SearchTerm { get; set; } = string.Empty;
        protected override int MinPageSize { get; set; } = PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.DEFAULT_MAX_PAGE_SIZE;
    }
}
