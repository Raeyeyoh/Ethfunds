public class PaymentStatusResponse
{
    public string TransactionReference { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int CampaignId { get; set; }
}