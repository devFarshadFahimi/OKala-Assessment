using Application.Features.CryptoMaps.Common;
using Domain.Aggregates;

namespace Application.Features.CryptoMaps.Commands.UpdateCryptoMaps;

public class UpdateCryptoMapsCommand(List<CryptocurrencyMapResponseDTO>? cryptocurrencyMaps) : IRequest<bool>
{
    public List<CryptocurrencyMapResponseDTO>? CryptocurrencyMaps { get; set; } = cryptocurrencyMaps ?? [];
}

public class UpdateCryptoMapsCommandHandler(IApplicationDbContext dbContext, ILogger<UpdateCryptoMapsCommandHandler> updateCryptoMapsCommandHandlerLogger) : IRequestHandler<UpdateCryptoMapsCommand, bool>
{
    public async Task<bool> Handle(UpdateCryptoMapsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            List<string> databaseExistedSymbols = dbContext.CryptoMaps.Select(p => p.Symbol).ToList() ?? [];

            List<CryptocurrencyMapResponseDTO> shouldInsert = request.CryptocurrencyMaps?.Where(p => !databaseExistedSymbols.Contains(p.Symbol)).ToList() ?? [];

            if (shouldInsert.Count > 0)
            {
                List<CryptoMapAggregate> newCryptoMaps = shouldInsert.Adapt<List<CryptoMapAggregate>>();
                await dbContext.CryptoMaps.AddRangeAsync(newCryptoMaps, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            updateCryptoMapsCommandHandlerLogger.LogInformation("Crypto map list updated successfully...!");
            return true;
        }
        catch (Exception ex)
        {
            updateCryptoMapsCommandHandlerLogger.LogWarning("Crypto map list updated failed for reason {@ExceptionMessage}", ex.Message);
            return false;
        }
    }
}
