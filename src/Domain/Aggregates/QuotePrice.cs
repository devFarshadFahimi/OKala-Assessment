using Domain.Abstractions;

namespace Domain.Aggregates;

public class QuotePrice : LongEntity
{
    public QuotePrice(string fiatPair, decimal value)
    {
        FiatPair = fiatPair;
        Value = value;
    }

    private QuotePrice() { }

    public string FiatPair { get; private set; } = string.Empty;
    public decimal Value { get; private set; }


    public long QuoteAggregateId { get; private set; }
    public QuoteAggregate QuoteAggregate { get; private set; } = null!;
}