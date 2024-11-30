namespace Application.Features.CryptoMaps.Common;

public class CryptocurrencyMapResponseDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("rank")]
    public int Rank { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("is_active")]
    public int IsActive { get; set; }
}


public class RequestedQuoteResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public Dictionary<string, QuoteDetail> Quote { get; set; } = [];


    public class QuoteDetail
    {
        public double? Price { get; set; }
    }
}
