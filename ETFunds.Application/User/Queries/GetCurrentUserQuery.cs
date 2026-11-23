using ETFunds.Application.Dtos;
using MediatR;

namespace ETFunds.Application.User.Queries;

public record GetCurrentUserQuery(int UserId) : IRequest<CurrentUserResponse?>;