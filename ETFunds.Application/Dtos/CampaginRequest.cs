namespace ETFunds.Application.Dtos;

public record CampaignRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal GoalAmount { get; set; }
    public Stream? Image { get; set; }
    public string? ImageFileName { get; set; }
    public int CategoryId { get; set; }
    public DateTime EndDate { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
}