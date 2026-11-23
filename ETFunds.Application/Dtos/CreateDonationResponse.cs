namespace ETFunds.Application.Dtos;

public class CreateDonationResponse
{
    public int DonationId { get; set; }
    public int PaymentId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string CheckoutUrl { get; set; } = string.Empty;
}