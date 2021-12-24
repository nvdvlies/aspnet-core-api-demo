using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

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
