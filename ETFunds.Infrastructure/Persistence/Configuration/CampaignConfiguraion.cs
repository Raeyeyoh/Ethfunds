using ETFunds.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ETFunds.Infrastructure.Persistence.Configuration;

public class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
{
    public void Configure(EntityTypeBuilder<Campaign> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(c => c.Description)
               .HasMaxLength(500);
        builder.HasOne(C => C.category)
             .WithMany(c => c.Campaigns)
             .HasForeignKey(c => c.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);

        //  builder.HasQueryFilter(x => !x.IsDeleted);
    }
}