using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

public class QuotePriceConfiguration : BaseLongConfiguration<QuotePrice>
{
    public override void Configure(EntityTypeBuilder<QuotePrice> builder)
    {
        base.Configure(builder);

        builder.HasOne(p => p.QuoteAggregate)
            .WithMany(p => p.Prices)
            .OnDelete(DeleteBehavior.Cascade);
    }
}