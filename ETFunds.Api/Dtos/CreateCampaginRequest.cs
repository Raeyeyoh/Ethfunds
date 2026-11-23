namespace ETFunds.Api.Dtos;


public class CreateCampaignRequest
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal GoalAmount { get; set; }

    public IFormFile? Image { get; set; }

    public int CategoryId { get; set; }

    public DateTime EndDate { get; set; }

    public string? Country { get; set; }

    public string? Region { get; set; }

    public string? City { get; set; }
}