using Domain.Aggregates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

public class QuoteAggregateConfiguration : BaseLongConfiguration<QuoteAggregate>
{
    public override void Configure(EntityTypeBuilder<QuoteAggregate> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.Name).HasMaxLength(100);
        builder.Property(p => p.Slug).HasMaxLength(100);
    }
}
