using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Auxilium.Core.Dtos;

namespace Auxilium.Core.Interfaces
{
	public interface ICosmosRepository<T> where T : Entity
	{
		Task UpsertItemsBatchAsync(IList<T> entities);
		Task UpsertItemsAsync(IList<T> entities);
		Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate = null,
			Expression<Func<T, T>> projection = null,
			Expression<Func<T, T>> orderBy = null,
			bool ascending = true,
			int limit = 0,
			bool firstPageOnly = false);

		Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate = null,
			Expression<Func<T, T>> projection = null);
	}
}