using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.CryptoMaps.Common;
namespace Infrastructure.Services;

public class CmcService : BaseHttpClientService, ICmcService
{
    public CmcService(IHttpClientFactory httpClientFactory, IOptions<CmcOptions> cmcOptions) : base(httpClientFactory, "CMC-Client")
    {
        _httpClient.BaseAddress = new Uri(cmcOptions.Value.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", cmcOptions.Value.ApiKey);
    }


    public async Task<CmsResponseWrapperDTO<List<CryptocurrencyMapResponseDTO>>?> CryptocurrencyMapAsync()
    {
        var response = await _httpClient.GetAsync("v1/cryptocurrency/map?sort=cmc_rank");

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("Service unavailable.");
        }

        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var result = JsonConvert.DeserializeObject<CmsResponseWrapperDTO<List<CryptocurrencyMapResponseDTO>>>(content);

        return result;
    }



    public async Task<CmsResponseWrapperDTO<List<Dictionary<string, List<RequestedQuoteResponseDTO>>>?>> GetCryptoQuoteAsync(string cryptoSymbol)
    {
        //var usdResult = _httpClient.GetAsync($"v2/cryptocurrency/quotes/latest?symbol={cryptoSymbol}");
        //var gbpResult = _httpClient.GetAsync($"v2/cryptocurrency/quotes/latest?symbol={cryptoSymbol}&convert=GBP");
        //var eurResult = _httpClient.GetAsync($"v2/cryptocurrency/quotes/latest?symbol={cryptoSymbol}&convert=EUR");
        //var audResult = _httpClient.GetAsync($"v2/cryptocurrency/quotes/latest?symbol={cryptoSymbol}&convert=AUD");
        //var brlResult = _httpClient.GetAsync($"v2/cryptocurrency/quotes/latest?symbol={cryptoSymbol}&convert=BRL");

        var tasks = new List<Task<HttpResponseMessage>>()
        {
             _httpClient.GetAsync($"v2/cryptocurrency/quotes/latest?symbol={cryptoSymbol}"),
             _httpClient.GetAsync($"v2/cryptocurrency/quotes/latest?symbol={cryptoSymbol}&convert=GBP"),
             _httpClient.GetAsync($"v2/cryptocurrency/quotes/latest?symbol={cryptoSymbol}&convert=EUR"),
             _httpClient.GetAsync($"v2/cryptocurrency/quotes/latest?symbol={cryptoSymbol}&convert=AUD"),
             _httpClient.GetAsync($"v2/cryptocurrency/quotes/latest?symbol={cryptoSymbol}&convert=BRL"),
        };

        var responses = await Task.WhenAll(tasks);

        if (responses.Any(p => !p.IsSuccessStatusCode)) // You can also filter out failed responses and just show success results.
        {
            throw new InvalidOperationException("Service unavailable.");
        }

        CmsResponseWrapperDTO<List<Dictionary<string, List<RequestedQuoteResponseDTO>>>>? result = new()
        {
            Data = []
        };
        foreach (var response in responses)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            CmsResponseWrapperDTO<Dictionary<string, List<RequestedQuoteResponseDTO>>>? aa = JsonConvert.DeserializeObject<CmsResponseWrapperDTO<Dictionary<string, List<RequestedQuoteResponseDTO>>>>(content);
            result.Data.Add(aa.Data);
        }

        return result;
    }
}

