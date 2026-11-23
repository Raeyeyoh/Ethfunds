
namespace ETFunds.Domain.Entities;

public class Campaign : BaseEntity
{

    public int Id { get; set; }
    public String Title { get; set; } = String.Empty;
    public String Description { get; set; } = String.Empty;
    public decimal GoalAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public String ImageUrl { get; set; } = String.Empty;
    public Category category { get; set; } = null!;
    public int CategoryId { get; set; }

    public int UserId { get; set; }
    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
    public User Creator { get; set; } = null!;
    public String Status { get; set; } = String.Empty;
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime EndDate { get; set; }
}