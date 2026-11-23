using ETFunds.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ETFunds.Infrastructure.Persistence.Configuration;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
       public void Configure(EntityTypeBuilder<Payment> builder)
       {
              builder.HasKey(p => p.Id);

              builder.Property(p => p.Amount)
                     .HasPrecision(18, 2);

              builder.Property(p => p.Currency)
                     .HasMaxLength(10)
                     .IsRequired();

              builder.Property(p => p.TransactionReference)
                     .HasMaxLength(100)
                     .IsRequired();

              builder.Property(p => p.ChapaReference)
                     .HasMaxLength(100);

              builder.Property(p => p.Status)
                     .HasMaxLength(30)
                     .IsRequired();

              builder.Property(p => p.PaymentMethod)
                     .HasMaxLength(50);

              builder.HasOne(p => p.Donation)
                     .WithOne(d => d.Payment)
                     .HasForeignKey<Payment>(p => p.DonationId)
                     .OnDelete(DeleteBehavior.Cascade);
       }
}