namespace ETFunds.Application.Dtos;

public class CampaignDetailsResponse
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal GoalAmount { get; set; }

    public decimal CurrentAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public string? Country { get; set; }

    public string? Region { get; set; }

    public string? City { get; set; }

    public string CreatorName { get; set; } = string.Empty;
}