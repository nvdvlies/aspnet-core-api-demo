using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;

namespace Demo.Domain.Shared.Interfaces
{
    internal interface IDomainEntityConfiguration<T> where T : IEntity
    {
        Func<IQueryable<T>, IIncludableQueryable<T, object>> Include { get; }
    }
}