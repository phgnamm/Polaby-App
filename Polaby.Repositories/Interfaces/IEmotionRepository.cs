using Polaby.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Interfaces
{
    public interface IEmotionRepository: IGenericRepository<Emotion>
    {
        Task<Emotion?> GetEmotionByDateAsync(Guid userID, DateOnly date);
    }
}
