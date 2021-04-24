using Demo.Domain.Shared.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;

namespace Demo.Domain.Shared.Interfaces
{
    internal interface IBusinessComponentConfiguration<T> where T : IEntity
    {
        Func<IQueryable<T>, IIncludableQueryable<T, object>> Include { get; }
    }
}
