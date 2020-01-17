using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using System.Linq;
using ArangoDB.Client.Query;

namespace Convey.Persistence.ArangoDB
{
    public static class Pagination
    {
        public static async Task<PagedResult<T>> PaginateAsync<T>(this IQueryable<T> collection, IPagedQuery query)
            => await collection.PaginateAsync(query.Page, query.Results);

        public static async Task<PagedResult<T>> PaginateAsync<T>(this IQueryable<T> collection, int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (resultsPerPage <= 0)
            {
                resultsPerPage = 10;
            }
            var isEmpty = await Task.Run(() => !collection.Any());
            if (isEmpty)
            {
                return PagedResult<T>.Empty;
            }
            var totalResults = await Task.Run(() => collection.Count());
            var totalPages = (int)Math.Ceiling((decimal)totalResults / resultsPerPage);
            var data = await Task.Run(() => collection.Skip(page).Take(resultsPerPage).ToList());

            return PagedResult<T>.Create(data, page, resultsPerPage, totalPages, totalResults);
        }

        public static ArangoQueryable<T> Limit<T>(this ArangoQueryable<T> collection, IPagedQuery query)
            => collection.Limit(query.Page, query.Results);

        public static ArangoQueryable<T> Limit<T>(this ArangoQueryable<T> collection,
            int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (resultsPerPage <= 0)
            {
                resultsPerPage = 10;
            }
            var skip = (page - 1) * resultsPerPage;
            var data = collection
                .Skip(skip)
                .Take(resultsPerPage);

            return data as ArangoQueryable<T>;
        }
    }
}