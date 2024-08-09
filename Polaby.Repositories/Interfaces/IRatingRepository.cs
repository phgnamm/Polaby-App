using Polaby.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Interfaces
{
    public interface IRatingRepository: IGenericRepository<Rating>
    {
        Task<Rating?> GetByUserAndExpertIdAsync(Guid userId, Guid expertId);
    }
}
