using System;
using System.Linq;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IDbQuery<T> where T : IEntity
    {
        IDbQuery<T> WithOptions(Action<IDbQueryOptions> action);
        IQueryable<T> AsQueryable();
    }
}