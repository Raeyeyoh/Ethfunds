namespace ETFunds.Application.Dtos;

public class CampaignResponse
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public decimal CurrentAmount { get; set; }

    public decimal GoalAmount { get; set; }
}