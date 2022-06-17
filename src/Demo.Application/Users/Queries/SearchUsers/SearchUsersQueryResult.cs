using System.Collections.Generic;
using Demo.Application.Shared.Models;
using Demo.Application.Users.Queries.SearchUsers.Dtos;

namespace Demo.Application.Users.Queries.SearchUsers
{
    public class SearchUsersQueryResult : BasePaginatedResult
    {
        public IEnumerable<SearchUserDto> Users { get; set; }
    }
}