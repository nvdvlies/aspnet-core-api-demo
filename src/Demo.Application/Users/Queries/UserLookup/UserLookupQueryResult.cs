using System.Collections.Generic;
using Demo.Application.Shared.Models;
using Demo.Application.Users.Queries.UserLookup.Dtos;

namespace Demo.Application.Users.Queries.UserLookup
{
    public class UserLookupQueryResult : BasePaginatedResult
    {
        public IEnumerable<UserLookupDto> Users { get; set; }
    }
}
