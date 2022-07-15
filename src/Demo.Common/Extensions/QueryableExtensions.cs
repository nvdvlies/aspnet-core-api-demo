using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Demo.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WriteQueryStringToOutputWindowIfInDebugMode<T>(this IQueryable<T> queryable)
        {
#if DEBUG
            Debug.WriteLine(queryable.ToQueryString());
#endif
            return queryable;
        }
    }
}
