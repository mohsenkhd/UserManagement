using Application.ViewModels.pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.Pagination
{
    public static class QueryableExtention
    {

        public static async Task<PagedResult<T>> GetPaged<T>(
           this IQueryable<T> query,
           int page,
           int pageSize,
           params Expression<Func<T, object>>?[] includes) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = await query.CountAsync()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var list = await query.ToListAsync();

            result.Results = list.Skip(skip).Take(pageSize).ToList();

            return result;
        }


    }
}
