
namespace ETFunds.Application.Interfaces;

using Domain.Entities;
using Dtos;

public interface IUserService
{
    Task<User> CreateUserAsync(CreateUserRequest user, CancellationToken ct);

    Task<LoginResult?> LoginAsync(
        LoginUserRequest request,
        CancellationToken ct);

    Task<CurrentUserResponse?> GetCurrentUserAsync(
        int userId,
        CancellationToken ct);

    Task<LoginResult?> RefreshTokenAsync(
        string refreshToken,
        CancellationToken ct);
}