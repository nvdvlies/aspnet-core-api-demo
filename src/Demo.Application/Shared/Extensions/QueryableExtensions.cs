using Demo.Application.Shared.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Demo.Application.Shared.Extensions
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, SortDirection sortDirection)
        {
            return sortDirection == SortDirection.Descending ?
                source.OrderByDescending(keySelector) :
                source.OrderBy(keySelector);
        }
    }
}
