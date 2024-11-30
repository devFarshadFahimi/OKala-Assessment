using Application.Common.Interfaces;
using Application.Features.CryptoMaps.Commands.UpdateCryptoMaps;
using MediatR;

namespace WebApi.HostedService;

public class CryptoCurrencyMapHostedService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CryptoCurrencyMapHostedService> _cryptoCurrencyMapHostedServiceLogger;
    public CryptoCurrencyMapHostedService(IServiceScopeFactory serviceScopeFactory,
        ILogger<CryptoCurrencyMapHostedService> cryptoCurrencyMapHostedServiceLogger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _cryptoCurrencyMapHostedServiceLogger = cryptoCurrencyMapHostedServiceLogger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await SyncCryptoMapsAsync();

        using var timer = new PeriodicTimer(TimeSpan.FromDays(1));
        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            using IServiceScope scope = await SyncCryptoMapsAsync();
        }
    }

    private async Task<IServiceScope> SyncCryptoMapsAsync()
    {
        var scope = _serviceScopeFactory.CreateScope();
        var cmcService = scope.ServiceProvider.GetRequiredService<ICmcService>();
        var cryptoMapsResponse = await cmcService.CryptocurrencyMapAsync();

        if (cryptoMapsResponse != null)
        {
            var mediatorService = scope.ServiceProvider.GetRequiredService<IMediator>();
            _ = await mediatorService.Send(new UpdateCryptoMapsCommand(cryptoMapsResponse.Data));
        }
        else
        {
            _cryptoCurrencyMapHostedServiceLogger.LogWarning("Crypto Maps Response was null in this call.");
        }

        return scope;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cryptoCurrencyMapHostedServiceLogger.LogInformation("Hosted service stopped at : {@CurrentDate}", DateTime.Now);

        return Task.CompletedTask;
    }
}
