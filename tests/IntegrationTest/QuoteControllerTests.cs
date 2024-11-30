using Application.Features.Quotes.Common;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace IntegrationTest;

public class QuoteControllerTests
{
    private readonly HttpClient client;

    public QuoteControllerTests()
    {
        client = new WebAppFactory().CreateClient();
    }
    [Fact]
    public async Task GetAndSubmitCryptoQuoteBySymbol_ShouldReturnExpectedSymbolData()
    {
        // Arrange
        var symbol = "BTT";

        // Act
        var response = await client.GetAsync($"/api/Quote/GetQuote?symbol={symbol}");
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        var actualResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);

        // Assert
        Assert.Equal(symbol, actualResponse.GetProperty("symbol").GetString());

        var prices = actualResponse.GetProperty("prices").EnumerateArray().ToList();
        Assert.Equal(5, prices.Count);

    }


    [Fact]
    public async Task GetLatestSubmittedQuotes_ShouldReturnExpectedSymbolData()
    {
        // Arrange
        string symbol = "BTT";
        int pageNumber = 1;
        int pageSize = 5;

        var requestUrl = $"/api/Quote/GetLatestSubmitedQuotes?symbol={symbol}&pageNumber={pageNumber}&pageSize={pageSize}";

        // Act
        var creationResponse = await client.GetAsync($"/api/Quote/GetQuote?symbol={symbol}");
        creationResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await client.GetAsync(requestUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var data = await response.Content.ReadFromJsonAsync<List<QuoteResponseDTO>>();

        data.Should().NotBeNull();
        data.Should().HaveCount(1);

        var firstItem = data[0];
        firstItem.Symbol.Should().Be("BTT");
        firstItem.Prices.Should().HaveCount(5);

        firstItem.Prices.Should().Contain(p => p.FiatPair == "USD" && p.Value > 0);
        firstItem.Prices.Should().Contain(p => p.FiatPair == "GBP" && p.Value > 0);
    }


}