using System.Dynamic;
using ETFunds.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace ETFunds.Infrastructure.Persistence.Data;

public class ETFDbContext(DbContextOptions<ETFDbContext> options) : DbContext(options)
{
    public DbSet<User> users => Set<User>();
    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<Category> categories => Set<Category>();
    public DbSet<Donation> Donations => Set<Donation>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.ApplyConfigurationsFromAssembly(
          typeof(ETFDbContext).Assembly);
    }
}

