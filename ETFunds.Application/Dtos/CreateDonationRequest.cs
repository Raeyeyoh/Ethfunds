namespace ETFunds.Application.Dtos;

public class CreateDonationRequest
{
    public int CampaignId { get; set; }
    public decimal Amount { get; set; }
    public bool IsAnonymous { get; set; }
}