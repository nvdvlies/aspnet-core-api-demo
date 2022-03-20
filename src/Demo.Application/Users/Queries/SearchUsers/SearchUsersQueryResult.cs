using Demo.Application.Shared.Models;
using Demo.Application.Users.Queries.SearchUsers.Dtos;
using System.Collections.Generic;

namespace Demo.Application.Users.Queries.SearchUsers
{
    public class SearchUsersQueryResult : BasePaginatedResult
    {
        public IEnumerable<SearchUserDto> Users { get; set; }
    }
}