namespace Application.Common.Models;

public class CmsResponseWrapperDTO<TData>
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("error_code")]
    public int ErrorCode { get; set; }

    [JsonPropertyName("elapsed")]
    public int Elapsed { get; set; }

    public TData Data { get; set; } = default!;
}
