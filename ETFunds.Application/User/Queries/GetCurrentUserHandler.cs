using ETFunds.Application.Dtos;
using ETFunds.Application.Interfaces;
using MediatR;

namespace ETFunds.Application.User.Queries;

public class GetCurrentUserHandler(
    IUserService userService)
    : IRequestHandler<GetCurrentUserQuery, CurrentUserResponse?>
{
    public async Task<CurrentUserResponse?> Handle(
        GetCurrentUserQuery request,
        CancellationToken ct)
    {
        return await userService.GetCurrentUserAsync(
            request.UserId,
            ct);
    }
}