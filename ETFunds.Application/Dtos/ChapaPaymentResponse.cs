namespace ETFunds.Application.Dtos;

public class ChapaPaymentResponse
{
    public bool Success { get; set; }
    public string? CheckoutUrl { get; set; }
    public string? ChapaReference { get; set; }
    public string? Message { get; set; }
}