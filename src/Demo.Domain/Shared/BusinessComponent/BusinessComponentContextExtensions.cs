using Demo.Domain.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Demo.Domain.Shared.BusinessComponent
{
    internal static class BusinessComponentContextExtensions
    {
        internal static bool IsPropertyDirty<TEntity, TProperty>(this IBusinessComponentContext<TEntity> context, Expression<Func<TEntity, TProperty>> property)
            where TEntity : IEntity
            where TProperty : IComparable
        {
            var currentValue = context.Entity != null ? property.Compile()(context.Entity) : default;
            var previousValue = context.Pristine != null ? property.Compile()(context.Pristine) : default;

            var comparer = EqualityComparer<TProperty>.Default;

            return comparer.Equals(currentValue, previousValue);
        }
    }
}
