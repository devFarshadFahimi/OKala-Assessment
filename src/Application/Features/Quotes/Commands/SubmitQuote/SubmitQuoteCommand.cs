using Application.Features.CryptoMaps.Common;
using Application.Features.Quotes.Common;
using Domain.Aggregates;

namespace Application.Features.Quotes.Commands.SubmitQuote;

public class SubmitQuoteCommand(string symbol) : IRequest<QuoteResponseDTO>
{
    public string Symbol { get; } = symbol;
}


internal class SubmitQuoteCommandHandler(IApplicationDbContext dbContext, ICmcService cmcService, ILogger<SubmitQuoteCommandHandler> submitQuoteCommandHandlerLogger)
    : IRequestHandler<SubmitQuoteCommand, QuoteResponseDTO>
{
    public async Task<QuoteResponseDTO> Handle(SubmitQuoteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var cryptoQuoteResponse = await cmcService.GetCryptoQuoteAsync(request.Symbol) ?? throw new InvalidOperationException();

            List<Dictionary<string, List<RequestedQuoteResponseDTO>>>? result = cryptoQuoteResponse.Data ?? throw new InvalidDataException();

            var firstItem = result.First();
            var firstKeyData = firstItem[firstItem.Keys.First()];

            var flatData = result
                .SelectMany(item => item.Values.First())
                .GroupBy(item => new { item.Id, item.Name, item.Symbol, item.Slug })
                .Select(group => new QuoteResponseDTO
                {
                    AssetId = group.Key.Id,
                    Name = group.Key.Name,
                    Symbol = group.Key.Symbol,
                    Slug = group.Key.Slug,
                    Prices = group
                    .SelectMany(item => item.Quote)
                    .Where(q => q.Value.Price.HasValue)
                    .Select(p => new QuotePriceDTO
                    {
                        FiatPair = p.Key,
                        Value = (decimal)p.Value.Price!.Value
                    })
                    .ToList()
                })
                .FirstOrDefault();

            if (flatData is not null)
            {
                var quoteAggregate = new QuoteAggregate(
                    assetId: flatData.AssetId,
                    name: flatData.Name,
                    symbol: flatData.Symbol,
                    slug: flatData.Slug);

                quoteAggregate.AddPrices(flatData.Prices.Adapt<List<QuotePrice>>());

                await dbContext.Quotes.AddAsync(quoteAggregate, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                submitQuoteCommandHandlerLogger.LogInformation("SubmitQuoteCommandHandler");

                return flatData;
            }

            throw new InvalidDataException();

        }
        catch (Exception ex)
        {
            submitQuoteCommandHandlerLogger.LogWarning("Submit Quote Request failed for reason : {@ExceptionMessage}", ex.Message);
            throw new InvalidOperationException();
        }
    }
}