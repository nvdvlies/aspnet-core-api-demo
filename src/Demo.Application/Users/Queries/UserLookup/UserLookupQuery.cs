using System;
using System.Web;
using Demo.Application.Shared.Interfaces;
using Demo.Application.Users.Queries.UserLookup.Dtos;
using MediatR;

namespace Demo.Application.Users.Queries.UserLookup;

public class UserLookupQuery : IQuery, IRequest<UserLookupQueryResult>
{
    public UserLookupOrderByEnum OrderBy { get; set; }
    public bool OrderByDescending { get; set; }
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public string SearchTerm { get; set; }
    public Guid[] Ids { get; set; }

    public override string ToString()
    {
        return ToQueryString();
    }

    public string ToQueryString()
    {
        var queryString = HttpUtility.ParseQueryString(string.Empty);

        queryString.Add(nameof(OrderBy), OrderBy.ToString());
        queryString.Add(nameof(OrderByDescending), OrderByDescending ? "true" : "false");
        queryString.Add(nameof(PageIndex), PageIndex.ToString());
        queryString.Add(nameof(PageSize), PageSize.ToString());
        queryString.Add(nameof(SearchTerm), SearchTerm);
        foreach (var id in Ids)
        {
            queryString.Add(nameof(Ids), id.ToString());
        }

        return queryString.ToString();
    }
}
