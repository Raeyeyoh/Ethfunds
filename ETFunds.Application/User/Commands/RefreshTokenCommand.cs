using ETFunds.Application.Dtos;
using ETFunds.Application.Interfaces;
using MediatR;

namespace ETFunds.Application.User.Commands;

public record RefreshTokenCommand(
    string RefreshToken
) : IRequest<LoginResult?>;

public class RefreshTokenHandler(
    IUserService userService)
    : IRequestHandler<RefreshTokenCommand, LoginResult?>
{
    public async Task<LoginResult?> Handle(
        RefreshTokenCommand request,
        CancellationToken ct)
    {
        return await userService.RefreshTokenAsync(
            request.RefreshToken,
            ct);
    }
}