using Application.Common.Models;
using Application.Features.CryptoMaps.Common;

namespace Application.Common.Interfaces;

public interface ICmcService
{
    Task<CmsResponseWrapperDTO<List<CryptocurrencyMapResponseDTO>>?> CryptocurrencyMapAsync();
    Task<CmsResponseWrapperDTO<List<Dictionary<string, List<RequestedQuoteResponseDTO>>>?>> GetCryptoQuoteAsync(string cryptoSymbol);
}
