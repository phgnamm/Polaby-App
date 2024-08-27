using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces
{
    public interface ICommentRepostiory : IGenericRepository<Comment>
    {
        Task<Comment> GetById(Guid id);
    }
}
