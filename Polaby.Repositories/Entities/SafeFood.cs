using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Entities
{
    public class SafeFood : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsSafe { get; set; }
        public string? Source { get; set; }
        public string? SourceUrl { get; set; }
    }
}
