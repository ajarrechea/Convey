using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Types;
using ArangoDB.Client;
using System.Linq;

namespace Convey.Persistence.ArangoDB.Repositories
{
	internal class ArangoRepository<TEntity, TIdentifiable> : IArangoRepository<TEntity, TIdentifiable>
		where TEntity : IIdentifiable<TIdentifiable>
	{
		public ArangoRepository(IArangoDatabase database)
		{
			Collection = database.Collection<TEntity>();
			Database = database;
		}

		public IArangoCollection<TEntity> Collection { get; }

		public IArangoDatabase Database { get; }

		public async Task<TEntity> GetAsync(TIdentifiable id)
			=> await GetAsync(e => e.Id.Equals(id));

		public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
			=> await Database.Query<TEntity>().Where(predicate).SingleOrDefaultAsync();

		public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
			=> await Database.Query<TEntity>().Where(predicate).ToListAsync();

		public async Task<PagedResult<TEntity>> BrowseAsync<TQuery>(Expression<Func<TEntity, bool>> predicate,
			TQuery query) where TQuery : IPagedQuery
			=> await Database.Query<TEntity>().Where(predicate).PaginateAsync(query);

		public async Task AddAsync(TEntity entity)
			=> await Database.InsertAsync<TEntity>(entity);

		public async Task UpdateAsync(TEntity entity)
			=> await Database.UpdateByIdAsync<TEntity>(entity.Id.ToString(), entity);

		public async Task DeleteAsync(TIdentifiable id)
			=> await Database.RemoveByIdAsync<TEntity>(id.ToString());

		public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
			=> await Task.Run(() => Database.Query<TEntity>().Where(predicate).Any());
	}
}