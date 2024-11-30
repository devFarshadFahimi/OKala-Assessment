namespace Infrastructure.Services;

public abstract class BaseHttpClientService
{
    protected readonly HttpClient _httpClient;

    protected BaseHttpClientService(IHttpClientFactory httpClientFactory, string clientName)
    {
        _httpClient = httpClientFactory.CreateClient(clientName);
    }
}
