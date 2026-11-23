using ETFunds.Application.Dtos;
using ETFunds.Application.Interfaces;
using ETFunds.Domain.Entities;
using ETFunds.Infrastructure.Persistence.Data;
using ETFunds.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ETFunds.Infrastructure.Services;

public class UserService(ETFDbContext _context, TokenService _tokenService) : IUserService
{

    public async Task<User> CreateUserAsync(
        CreateUserRequest user,
        CancellationToken ct)
    {
        var newUser = new User
        {
            FullName = user.FullName,
            Email = user.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
            Role = Role.User
        };

        await _context.users.AddAsync(newUser, ct);
        await _context.SaveChangesAsync(ct);

        return newUser;
    }

    public async Task<LoginResult?> LoginAsync(
      LoginUserRequest request,
      CancellationToken ct)
    {
        var user = await _context.users
            .FirstOrDefaultAsync(
                u => u.Email == request.Email, ct);

        if (user == null)
        {
            return new LoginResult(
                false,
                null,
                null);
        }

        var validPassword = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash);

        if (!validPassword)
        {
            return new LoginResult(
                false,
                null,
                null);
        }

        var accessToken = _tokenService.GenerateJwt(user);

        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString("N"),
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsUsed = false,
            IsRevoked = false
        };

        _context.RefreshTokens.Add(refreshToken);

        await _context.SaveChangesAsync(ct);

        return new LoginResult(
            true,
            accessToken,
            refreshToken.Token);
    }
    public async Task<CurrentUserResponse?> GetCurrentUserAsync(
    int userId,
    CancellationToken ct)
    {
        var user = await _context.users
            .FirstOrDefaultAsync(
                u => u.Id == userId,
                ct);

        if (user == null)
            return null;

        return new CurrentUserResponse(
            user.Id,
            user.FullName,
            user.Email,
            user.Role.ToString()
        );
    }
    public async Task<LoginResult?> RefreshTokenAsync(
    string refreshToken,
    CancellationToken ct)
    {
        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(
                rt => rt.Token == refreshToken,
                ct);

        if (storedToken == null)
            return null;

        // Token reuse detection
        if (storedToken.IsUsed)
        {
            var userTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == storedToken.UserId)
                .ToListAsync(ct);

            foreach (var token in userTokens)
            {
                token.IsRevoked = true;
            }

            await _context.SaveChangesAsync(ct);

            return null;
        }

        // Expired or revoked
        if (storedToken.IsRevoked ||
            storedToken.ExpiresAt <= DateTime.UtcNow)
        {
            return null;
        }

        // Find user
        var user = await _context.users
            .FirstOrDefaultAsync(
                u => u.Id == storedToken.UserId,
                ct);

        if (user == null)
            return null;

        // Mark old refresh token as used
        storedToken.IsUsed = true;

        // Generate new access token
        var newAccessToken =
            _tokenService.GenerateJwt(user);

        // Generate new refresh token
        var newRefreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString("N"),
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsUsed = false,
            IsRevoked = false
        };

        _context.RefreshTokens.Add(newRefreshToken);

        await _context.SaveChangesAsync(ct);

        return new LoginResult(
            true,
            newAccessToken,
            newRefreshToken.Token);
    }
}