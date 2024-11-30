using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<CryptoMapAggregate> CryptoMaps { get; set; }
    DbSet<QuoteAggregate> Quotes { get; set; }
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
