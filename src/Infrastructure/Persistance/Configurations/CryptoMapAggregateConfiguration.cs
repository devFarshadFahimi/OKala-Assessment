using Domain.Aggregates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

public class CryptoMapAggregateConfiguration : BaseLongConfiguration<CryptoMapAggregate>
{
    public override void Configure(EntityTypeBuilder<CryptoMapAggregate> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.Name).HasMaxLength(100);
        builder.Property(p => p.Slug).HasMaxLength(100);
    }
}
