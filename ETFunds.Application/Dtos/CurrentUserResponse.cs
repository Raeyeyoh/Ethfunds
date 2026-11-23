namespace ETFunds.Application.Dtos;

public record CurrentUserResponse(
    int Id,
    string FullName,
    string Email,
    string Role
);