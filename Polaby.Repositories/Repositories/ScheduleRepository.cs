using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Repositories
{
    public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
    {
        private readonly AppDbContext _dbContext;

        public ScheduleRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<Schedule> GetById(Guid id)
        {
            return await _dbContext.Schedule
                .Include(cp => cp.User)
                .FirstOrDefaultAsync(cp => cp.Id == id);
        }
    }
}
