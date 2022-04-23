using Demo.Application.Shared.Interfaces;
using MediatR;
using System;

namespace Demo.Application.UserPreferences.Queries.GetUserPreferences
{
    public class GetUserPreferencesQuery : IQuery, IRequest<GetUserPreferencesQueryResult>
    {
    }
}