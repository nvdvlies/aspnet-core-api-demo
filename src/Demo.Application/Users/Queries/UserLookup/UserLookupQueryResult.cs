using Demo.Application.Shared.Models;
using Demo.Application.Users.Queries.UserLookup.Dtos;
using System.Collections.Generic;

namespace Demo.Application.Users.Queries.UserLookup
{
    public class UserLookupQueryResult : BasePaginatedResult
    {
        public IEnumerable<UserLookupDto> Users { get; set; }
    }
}