using System.Linq.Expressions;
using Polaby.Repositories.Models.AccountModels;
using Polaby.Repositories.Models.QueryModels;
using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces
{
	public interface IAccountRepository
	{
		Task<QueryResultModel<List<AccountModel>>> GetAllAsync(
			Expression<Func<AccountModel, bool>>? filter = null,
			Func<IQueryable<AccountModel>, IOrderedQueryable<AccountModel>>? orderBy = null,
			string include = "",
			int? pageIndex = null,
			int? pageSize = null
		);
		Task<Account> GetAccountById(Guid id);

    }
}
