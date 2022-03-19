using Demo.Application.Shared.Models;
using Demo.Application.Users.Queries.SearchUsers.Dto;
using System.Collections.Generic;

namespace Demo.Application.Users.Queries.SearchUsers
{
    public class SearchUsersQueryResult : BasePaginatedResult
    {
        public IEnumerable<SearchUserDto> Users { get; set; }
    }
}