using Domain.Aggregates;

namespace Application.Features.CryptoMaps.Common;

internal class CryptoMapsMappers : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CryptocurrencyMapResponseDTO, CryptoMapAggregate>()
            .Ignore(p => p.Id)
            .Map(p => p.AssetId, src => src.Id)
        ;
    }
}
