namespace ETFunds.Application.Dtos;

public record CreateUserRequest
(
     string FullName,
     string Email,
     string Password);
