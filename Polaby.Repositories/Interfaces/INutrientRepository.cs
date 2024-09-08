
using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces
{
    public interface INutrientRepository : IGenericRepository<Nutrient>
    {
        public IQueryable<Nutrient> GetAll();
    }
}
