using System.Linq;
using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.User;

namespace Demo.Application.Users.Queries.IsEmailAvailable
{
    public class IsEmailAvailableQueryHandler : IRequestHandler<IsEmailAvailableQuery, IsEmailAvailableQueryResult>
    {
        private readonly IDbQuery<User> _query;

        public IsEmailAvailableQueryHandler(IDbQuery<User> query)
        {
            _query = query;
        }

        public async Task<IsEmailAvailableQueryResult> Handle(IsEmailAvailableQuery request, CancellationToken cancellationToken)
        {
            var query = _query.AsQueryable();

            if (request.IgnoreId.HasValue)
            {
                query = query.Where(x => x.Id != request.IgnoreId.Value);
            }

            var exists = await query.AnyAsync(x => x.Email == request.Email, cancellationToken);

            return new IsEmailAvailableQueryResult
            {
                IsEmailAvailable = !exists
            };
        }
    }
}