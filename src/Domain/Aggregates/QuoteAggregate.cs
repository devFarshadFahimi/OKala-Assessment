using Domain.Abstractions;

namespace Domain.Aggregates;

/// <summary>
/// This is not all properties that CMC service returns, these are just required properties.
/// </summary>
public class QuoteAggregate : AggregateRoot
{
    public QuoteAggregate(int assetId, string name, string symbol, string slug)
    {
        AssetId = assetId;
        Name = name;
        Symbol = symbol;
        Slug = slug;
        SubmitDate = DateTime.Now;
    }

    private QuoteAggregate()
    {

    }

    public int AssetId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Symbol { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public DateTime SubmitDate { get; private set; }

    public ICollection<QuotePrice> Prices { get; private set; } = [];


    public void AddPrices(List<QuotePrice> prices)
    {
        foreach (var item in prices)
        {
            Prices.Add(item);
        }
    }

}
