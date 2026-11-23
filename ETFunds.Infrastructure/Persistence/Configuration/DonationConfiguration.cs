using ETFunds.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ETFunds.Infrastructure.Persistence.Configuration;

public class DonationConfiguration : IEntityTypeConfiguration<Donation>
{
    public void Configure(EntityTypeBuilder<Donation> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Amount)
     .HasPrecision(18, 2);
        builder.HasOne(d => d.Campaign)
            .WithMany(c => c.Donations)
            .HasForeignKey(d => d.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.User)
            .WithMany(u => u.Donations)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);


    }
}