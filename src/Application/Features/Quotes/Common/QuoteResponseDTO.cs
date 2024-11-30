using Domain.Aggregates;

namespace Application.Features.Quotes.Common;

public class QuoteResponseDTO
{
    [JsonIgnore]
    public int AssetId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public List<QuotePriceDTO> Prices { get; set; } = [];
}
public class QuotePriceDTO
{
    public string FiatPair { get; set; } = string.Empty;
    public decimal Value { get; set; }
}


internal class QuoteMappers : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<QuoteAggregate, QuoteResponseDTO>()
            .Map(p => p.AssetId, src => src.Id)
        ;
    }
}
