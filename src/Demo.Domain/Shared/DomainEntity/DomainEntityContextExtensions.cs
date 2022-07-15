using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Shared.DomainEntity
{
    internal static class DomainEntityContextExtensions
    {
        internal static bool IsPropertyDirty<TEntity, TProperty>(this IDomainEntityContext<TEntity> context,
            Expression<Func<TEntity, TProperty>> property)
            where TEntity : IEntity
            where TProperty : IComparable
        {
            var currentValue = context.Entity != null ? property.Compile()(context.Entity) : default;
            var previousValue = context.Pristine != null ? property.Compile()(context.Pristine) : default;

            var comparer = EqualityComparer<TProperty>.Default;

            return !comparer.Equals(currentValue, previousValue);
        }
    }
}
