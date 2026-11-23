namespace ETFunds.Application.Dtos;

public record LoginResult(
    bool Success,
    string? AccessToken,
    string? RefreshToken
);