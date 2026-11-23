
using ETFunds.Domain.Enums;

namespace ETFunds.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string PasswordHash { get; set; } = String.Empty;
    public Role Role { get; set; }
    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
}

