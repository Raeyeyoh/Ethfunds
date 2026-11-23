namespace ETFunds.Domain.Entities;

public class Donation
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public int CampaignId { get; set; }
    public int UserId { get; set; }
    public Campaign Campaign { get; set; } = null!;
    public User User { get; set; } = null!;
    public DateTime DonatedAt { get; set; }
    public bool IsAnonymous { get; set; }
    public Payment? Payment { get; set; }

}
