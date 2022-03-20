using Demo.Application.Roles.Queries.RoleLookup.Dtos;
using Demo.Application.Shared.Models;
using System.Collections.Generic;

namespace Demo.Application.Roles.Queries.RoleLookup
{
    public class RoleLookupQueryResult : BasePaginatedResult
    {
        public IEnumerable<RoleLookupDto> Roles { get; set; }
    }
}