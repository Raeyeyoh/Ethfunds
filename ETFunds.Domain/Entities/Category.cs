namespace ETFunds.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();
}
