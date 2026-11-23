using ETFunds.Application.Dtos;
using ETFunds.Application.Interfaces;
using MediatR;

namespace ETFunds.Application.User.Commands;

public record RegisterUserCommand(CreateUserRequest Request) : IRequest<bool>;
public class RegisterUserHandler(IUserService userService) : IRequestHandler<RegisterUserCommand, bool>
{
    public async Task<bool> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        await userService.CreateUserAsync(request.Request, ct);
        return true;
    }
}