using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Repositories
{
    public class CommunityPostRepository : GenericRepository<CommunityPost>, ICommuntityPostRepository
    {
        private readonly AppDbContext _dbContext;

        public CommunityPostRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

    }
}
