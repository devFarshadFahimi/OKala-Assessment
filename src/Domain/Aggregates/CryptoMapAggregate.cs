using Domain.Abstractions;

namespace Domain.Aggregates;

/// <summary>
/// This is not all properties that CMC service returns, these are just required properties.
/// </summary>
public class CryptoMapAggregate : AggregateRoot
{
    public CryptoMapAggregate(int assetId, int rank, string name, string symbol, string slug, int isActive)
    {
        AssetId = assetId;
        Rank = rank;
        Name = name;
        Symbol = symbol;
        Slug = slug;
        IsActive = isActive;
    }

    private CryptoMapAggregate() { }

    public int AssetId { get; private set; }
    public int Rank { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Symbol { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public int IsActive { get; private set; }
}
