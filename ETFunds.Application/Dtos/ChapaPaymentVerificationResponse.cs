namespace ETFunds.Application.Dtos;

public class ChapaPaymentVerificationResponse
{
    public bool Success { get; set; }
    public string? TransactionReference { get; set; }
    public string? Status { get; set; }
    public decimal Amount { get; set; }
    public string? Currency { get; set; }
    public string? ChapaReference { get; set; }
    public string? Message { get; set; }
}