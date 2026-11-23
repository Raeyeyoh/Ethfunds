using ETFunds.Application.Dtos;
using ETFunds.Application.Interfaces;
using MediatR;


namespace ETFunds.Application.User.Commands;

public record LoginUserCommand(LoginUserRequest Request)
    : IRequest<LoginResult>;

public class LoginUserHandler(IUserService userService) : IRequestHandler<LoginUserCommand, LoginResult>
{
    public async Task<LoginResult> Handle(
    LoginUserCommand request,
    CancellationToken ct)
    {
        var result = await userService.LoginAsync(
            request.Request,
            ct);

        return result
            ?? new LoginResult(false, null, null);
    }
}