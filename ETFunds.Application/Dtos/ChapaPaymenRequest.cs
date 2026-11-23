namespace ETFunds.Application.Dtos;

public class ChapaPaymentRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "ETB";
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
}