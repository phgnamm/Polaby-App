using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Repositories
{
    public class NoteRepository : GenericRepository<Note>, INoteRepository
    {
        public NoteRepository(AppDbContext dbContext, IClaimsService claimsService) :
            base(dbContext, claimsService)
        {
        }
    }
}
