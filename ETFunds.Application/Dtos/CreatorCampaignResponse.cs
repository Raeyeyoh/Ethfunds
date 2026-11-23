namespace ETFunds.Application.Dtos;

public class CreatorCampaignResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal GoalAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime EndDate { get; set; }
    public string? ImageUrl { get; set; }
}